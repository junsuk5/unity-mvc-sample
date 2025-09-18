using System.Collections.Generic;
using System.Threading.Tasks;
using Common.StatementSystem;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class StateMachineTests
    {
        private abstract class TestState : IState<TestState>
        {
            protected readonly List<string> log;

            protected TestState(List<string> log)
            {
                this.log = log;
            }

            public abstract UniTask<TestState> Enter(TestState previousState);

            public virtual UniTask Exit(TestState nextState)
            {
                return UniTask.CompletedTask;
            }
        }

        private sealed class StartState : TestState
        {
            private readonly int repeatCount;

            public StartState(List<string> log, int repeatCount) : base(log)
            {
                this.repeatCount = repeatCount;
            }

            public override UniTask<TestState> Enter(TestState previousState)
            {
                log.Add("Enter:Start");
                return UniTask.FromResult<TestState>(new CountingState(log, repeatCount));
            }

            public override UniTask Exit(TestState nextState)
            {
                log.Add($"Exit:Start -> {nextState?.GetType().Name}");
                return UniTask.CompletedTask;
            }
        }

        private sealed class CountingState : TestState
        {
            private readonly int remaining;

            public CountingState(List<string> log, int remaining) : base(log)
            {
                this.remaining = remaining;
            }

            public override async UniTask<TestState> Enter(TestState previousState)
            {
                log.Add($"Enter:Count({remaining})");
                await UniTask.Yield();

                if (remaining > 1)
                {
                    return new CountingState(log, remaining - 1);
                }

                return new EndState(log);
            }

            public override UniTask Exit(TestState nextState)
            {
                log.Add($"Exit:Count({remaining}) -> {nextState?.GetType().Name}");
                return UniTask.CompletedTask;
            }
        }

        private sealed class EndState : TestState
        {
            public EndState(List<string> log) : base(log)
            {
            }

            public override UniTask<TestState> Enter(TestState previousState)
            {
                log.Add("Enter:End");
                return UniTask.FromResult<TestState>(null);
            }
        }

        [Test]
        public async Task Execute_상태전이순서_확인()
        {
            var log = new List<string>();
            var stateMachine = new StateMachine(isLogging: false);

            await stateMachine.Execute<TestState>(new StartState(log, repeatCount: 2));

            CollectionAssert.AreEqual(new[]
            {
                "Enter:Start",
                "Exit:Start -> CountingState",
                "Enter:Count(2)",
                "Exit:Count(2) -> CountingState",
                "Enter:Count(1)",
                "Exit:Count(1) -> EndState",
                "Enter:End"
            },
            log);
        }
    }
}
