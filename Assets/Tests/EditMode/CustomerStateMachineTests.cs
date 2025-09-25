using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.StatementSystem;
using Cysharp.Threading.Tasks;
using Feature._3DGame.GamePlay;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public class CustomerStateMachineTests
    {
        private class TestableCustomer
        {
            public CustomerStateType CurrentStateType { get; private set; } = CustomerStateType.Waiting;
            public float Patience { get; private set; } = 100f;
            public float MaxPatience { get; private set; } = 100f;
            public float SatisfactionLevel { get; private set; } = 0f;
            public bool IsPatienceDecaying { get; private set; } = false;

            private readonly List<string> eventLog = new List<string>();

            public List<string> EventLog => eventLog;

            public void SetCurrentStateType(CustomerStateType stateType)
            {
                CurrentStateType = stateType;
                eventLog.Add($"StateChanged:{stateType}");
            }

            public void StartPatienceDecay()
            {
                IsPatienceDecaying = true;
                eventLog.Add("StartPatienceDecay");
            }

            public void StopPatienceDecay()
            {
                IsPatienceDecaying = false;
                eventLog.Add("StopPatienceDecay");
            }

            public void CompleteOrder(float serviceQuality)
            {
                float patienceRatio = Patience / MaxPatience;
                SatisfactionLevel = (patienceRatio * 0.6f + serviceQuality * 0.4f) * 100f;
                eventLog.Add($"OrderCompleted:Satisfaction={SatisfactionLevel:F1}");
            }

            public void LeaveRestaurant(bool wasSatisfied)
            {
                eventLog.Add($"Left:Satisfied={wasSatisfied}");
            }

            public void SetPatience(float patience)
            {
                Patience = Mathf.Clamp(patience, 0f, MaxPatience);
            }
        }

        // Test Customer States with TestableCustomer
        private class TestWaitingState : CustomerState
        {
            private readonly TestableCustomer testCustomer;
            private readonly bool shouldTimeout;

            public TestWaitingState(TestableCustomer customer, bool shouldTimeout = false) : base(null, CustomerStateType.Waiting)
            {
                this.testCustomer = customer;
                this.shouldTimeout = shouldTimeout;
            }

            public override async UniTask<CustomerState> Enter(CustomerState previousState)
            {
                testCustomer.SetCurrentStateType(CustomerStateType.Waiting);
                testCustomer.StartPatienceDecay();

                // Simulate timeout if needed
                if (shouldTimeout)
                {
                    testCustomer.SetPatience(0f);
                    return new TestLeavingState(testCustomer, false);
                }

                await UniTask.Yield();
                return new TestOrderingState(testCustomer);
            }

            public override async UniTask Exit(CustomerState nextState)
            {
                await UniTask.CompletedTask;
            }
        }

        private class TestOrderingState : CustomerState
        {
            private readonly TestableCustomer testCustomer;

            public TestOrderingState(TestableCustomer customer) : base(null, CustomerStateType.Ordering)
            {
                this.testCustomer = customer;
            }

            public override async UniTask<CustomerState> Enter(CustomerState previousState)
            {
                testCustomer.SetCurrentStateType(CustomerStateType.Ordering);

                await UniTask.Yield();

                if (testCustomer.Patience <= 0f)
                {
                    return new TestLeavingState(testCustomer, false);
                }

                testCustomer.CompleteOrder(0.8f);
                return new TestEatingState(testCustomer);
            }

            public override async UniTask Exit(CustomerState nextState)
            {
                await UniTask.CompletedTask;
            }
        }

        private class TestEatingState : CustomerState
        {
            private readonly TestableCustomer testCustomer;

            public TestEatingState(TestableCustomer customer) : base(null, CustomerStateType.Eating)
            {
                this.testCustomer = customer;
            }

            public override async UniTask<CustomerState> Enter(CustomerState previousState)
            {
                testCustomer.SetCurrentStateType(CustomerStateType.Eating);
                testCustomer.StopPatienceDecay();

                await UniTask.Yield();
                return new TestPayingState(testCustomer);
            }

            public override async UniTask Exit(CustomerState nextState)
            {
                await UniTask.CompletedTask;
            }
        }

        private class TestPayingState : CustomerState
        {
            private readonly TestableCustomer testCustomer;

            public TestPayingState(TestableCustomer customer) : base(null, CustomerStateType.Paying)
            {
                this.testCustomer = customer;
            }

            public override async UniTask<CustomerState> Enter(CustomerState previousState)
            {
                testCustomer.SetCurrentStateType(CustomerStateType.Paying);

                await UniTask.Yield();
                return new TestLeavingState(testCustomer, true);
            }

            public override async UniTask Exit(CustomerState nextState)
            {
                await UniTask.CompletedTask;
            }
        }

        private class TestLeavingState : CustomerState
        {
            private readonly TestableCustomer testCustomer;
            private readonly bool wasSatisfied;

            public TestLeavingState(TestableCustomer customer, bool wasSatisfied) : base(null, CustomerStateType.Leaving)
            {
                this.testCustomer = customer;
                this.wasSatisfied = wasSatisfied;
            }

            public override async UniTask<CustomerState> Enter(CustomerState previousState)
            {
                testCustomer.SetCurrentStateType(CustomerStateType.Leaving);
                testCustomer.LeaveRestaurant(wasSatisfied);

                await UniTask.Yield();
                return null; // End state machine
            }

            public override async UniTask Exit(CustomerState nextState)
            {
                await UniTask.CompletedTask;
            }
        }

        [Test]
        public async Task CustomerStateMachine_정상플로우_완주테스트()
        {
            // Arrange
            var testCustomer = new TestableCustomer();
            var stateMachine = new StateMachine(false);

            // Act
            await stateMachine.Execute<CustomerState>(new TestWaitingState(testCustomer));

            // Assert
            var expectedEvents = new[]
            {
                "StateChanged:Waiting",
                "StartPatienceDecay",
                "StateChanged:Ordering",
                "StateChanged:Eating",
                "StopPatienceDecay",
                "StateChanged:Paying",
                "StateChanged:Leaving",
                "Left:Satisfied=True"
            };

            // 만족도 계산 이벤트는 별도로 검증 (값이 가변적)
            Assert.That(testCustomer.EventLog.Any(log => log.StartsWith("OrderCompleted:Satisfaction=")),
                "OrderCompleted 이벤트가 발생해야 합니다");

            // 기본 이벤트 순서 검증 (만족도 값 제외)
            var filteredEvents = testCustomer.EventLog.Where(log => !log.StartsWith("OrderCompleted:")).ToList();
            CollectionAssert.AreEqual(expectedEvents, filteredEvents);
            Assert.AreEqual(CustomerStateType.Leaving, testCustomer.CurrentStateType);
            Assert.That(testCustomer.SatisfactionLevel, Is.InRange(80f, 100f), "만족도는 80-100% 범위여야 합니다");
        }

        [Test]
        public async Task CustomerStateMachine_타임아웃플로우_불만족퇴장테스트()
        {
            // Arrange
            var testCustomer = new TestableCustomer();
            var stateMachine = new StateMachine(false);

            // Act - timeout during waiting
            await stateMachine.Execute<CustomerState>(new TestWaitingState(testCustomer, shouldTimeout: true));

            // Assert
            var expectedEvents = new[]
            {
                "StateChanged:Waiting",
                "StartPatienceDecay",
                "StateChanged:Leaving",
                "Left:Satisfied=False"
            };

            CollectionAssert.AreEqual(expectedEvents, testCustomer.EventLog);
            Assert.AreEqual(CustomerStateType.Leaving, testCustomer.CurrentStateType);
            Assert.AreEqual(0f, testCustomer.Patience);
        }

        [Test]
        public async Task CustomerStateMachine_식사중인내심중단_확인테스트()
        {
            // Arrange
            var testCustomer = new TestableCustomer();
            var stateMachine = new StateMachine(false);

            // Act
            await stateMachine.Execute<CustomerState>(new TestWaitingState(testCustomer));

            // Assert - patience decay should be stopped during eating
            Assert.Contains("StopPatienceDecay", testCustomer.EventLog);
            Assert.IsFalse(testCustomer.IsPatienceDecaying);
        }

        [Test]
        public async Task CustomerStateMachine_만족도계산_정확성테스트()
        {
            // Arrange
            var testCustomer = new TestableCustomer();
            testCustomer.SetPatience(80f); // 80% patience remaining

            // Act
            testCustomer.CompleteOrder(1.0f); // Perfect service

            // Assert
            // Expected: (0.8 * 0.6 + 1.0 * 0.4) * 100 = (0.48 + 0.4) * 100 = 88.0
            Assert.AreEqual(88.0f, testCustomer.SatisfactionLevel, 0.1f);
        }

        [Test]
        public void CustomerStateType_모든상태_열거형확인테스트()
        {
            // Arrange & Act
            var allStates = System.Enum.GetValues(typeof(CustomerStateType));

            // Assert
            Assert.AreEqual(5, allStates.Length);
            Assert.IsTrue(System.Enum.IsDefined(typeof(CustomerStateType), CustomerStateType.Waiting));
            Assert.IsTrue(System.Enum.IsDefined(typeof(CustomerStateType), CustomerStateType.Ordering));
            Assert.IsTrue(System.Enum.IsDefined(typeof(CustomerStateType), CustomerStateType.Eating));
            Assert.IsTrue(System.Enum.IsDefined(typeof(CustomerStateType), CustomerStateType.Paying));
            Assert.IsTrue(System.Enum.IsDefined(typeof(CustomerStateType), CustomerStateType.Leaving));
        }

        [Test]
        public async Task CustomerStateMachine_상태전환순서_검증테스트()
        {
            // Arrange
            var testCustomer = new TestableCustomer();
            var stateMachine = new StateMachine(false);

            // Act
            await stateMachine.Execute<CustomerState>(new TestWaitingState(testCustomer));

            // Assert - verify correct state transition order
            var stateChanges = testCustomer.EventLog
                .Where(log => log.StartsWith("StateChanged:"))
                .Select(log => (CustomerStateType)System.Enum.Parse(typeof(CustomerStateType), log.Split(':')[1]))
                .ToArray();

            var expectedOrder = new[]
            {
                CustomerStateType.Waiting,
                CustomerStateType.Ordering,
                CustomerStateType.Eating,
                CustomerStateType.Paying,
                CustomerStateType.Leaving
            };

            CollectionAssert.AreEqual(expectedOrder, stateChanges);
        }
    }
}