using System;
using Common.EventSystem;
using Common.StatementSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Feature._3DGame.GamePlay
{
    public struct CustomerSpawnedEvent : IEvent
    {
        public Customer Customer { get; }
        public Vector3 SpawnPosition { get; }

        public CustomerSpawnedEvent(Customer customer, Vector3 spawnPosition)
        {
            Customer = customer;
            SpawnPosition = spawnPosition;
        }
    }

    public struct CustomerOrderCompletedEvent : IEvent
    {
        public Customer Customer { get; }
        public float SatisfactionLevel { get; }

        public CustomerOrderCompletedEvent(Customer customer, float satisfactionLevel)
        {
            Customer = customer;
            SatisfactionLevel = satisfactionLevel;
        }
    }

    public struct CustomerLeftEvent : IEvent
    {
        public Customer Customer { get; }
        public bool WasSatisfied { get; }

        public CustomerLeftEvent(Customer customer, bool wasSatisfied)
        {
            Customer = customer;
            WasSatisfied = wasSatisfied;
        }
    }

    public enum CustomerStateType
    {
        Waiting,
        Ordering,
        Eating,
        Paying,
        Leaving
    }

    // Base Customer State
    public abstract class CustomerState : IState<CustomerState>
    {
        protected Customer customer;
        protected CustomerStateType stateType;

        public CustomerState(Customer customer, CustomerStateType stateType)
        {
            this.customer = customer;
            this.stateType = stateType;
        }

        public abstract UniTask<CustomerState> Enter(CustomerState previousState);
        public abstract UniTask Exit(CustomerState nextState);
    }

    // Waiting State
    public class WaitingState : CustomerState
    {
        public WaitingState(Customer customer) : base(customer, CustomerStateType.Waiting)
        {
        }

        public override async UniTask<CustomerState> Enter(CustomerState previousState)
        {
            customer.SetCurrentStateType(CustomerStateType.Waiting);

            // 대기 중 인내심 감소 시작
            customer.StartPatienceDecay();

            // 대기 시간 후 주문 상태로 전환 또는 떠나기
            float waitTime = UnityEngine.Random.Range(2f, 5f);
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

            if (customer.Patience <= 0f)
            {
                return new LeavingState(customer, false); // 불만족으로 떠나기
            }

            return new OrderingState(customer);
        }

        public override async UniTask Exit(CustomerState nextState)
        {
            await UniTask.CompletedTask;
        }
    }

    // Ordering State
    public class OrderingState : CustomerState
    {
        public OrderingState(Customer customer) : base(customer, CustomerStateType.Ordering)
        {
        }

        public override async UniTask<CustomerState> Enter(CustomerState previousState)
        {
            customer.SetCurrentStateType(CustomerStateType.Ordering);

            // 주문 처리 대기
            float orderTime = UnityEngine.Random.Range(3f, 7f);
            await UniTask.Delay(TimeSpan.FromSeconds(orderTime));

            if (customer.Patience <= 0f)
            {
                return new LeavingState(customer, false);
            }

            // 주문 완료 후 식사 상태로 전환
            float serviceQuality = UnityEngine.Random.Range(0.5f, 1f);
            customer.CompleteOrder(serviceQuality);

            return new EatingState(customer);
        }

        public override async UniTask Exit(CustomerState nextState)
        {
            await UniTask.CompletedTask;
        }
    }

    // Eating State
    public class EatingState : CustomerState
    {
        public EatingState(Customer customer) : base(customer, CustomerStateType.Eating)
        {
        }

        public override async UniTask<CustomerState> Enter(CustomerState previousState)
        {
            customer.SetCurrentStateType(CustomerStateType.Eating);
            customer.StopPatienceDecay(); // 식사 중에는 인내심 감소 중단

            // 식사 시간
            float eatingTime = UnityEngine.Random.Range(5f, 10f);
            await UniTask.Delay(TimeSpan.FromSeconds(eatingTime));

            return new PayingState(customer);
        }

        public override async UniTask Exit(CustomerState nextState)
        {
            await UniTask.CompletedTask;
        }
    }

    // Paying State
    public class PayingState : CustomerState
    {
        public PayingState(Customer customer) : base(customer, CustomerStateType.Paying)
        {
        }

        public override async UniTask<CustomerState> Enter(CustomerState previousState)
        {
            customer.SetCurrentStateType(CustomerStateType.Paying);

            // 결제 시간
            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            return new LeavingState(customer, true); // 만족으로 떠나기
        }

        public override async UniTask Exit(CustomerState nextState)
        {
            await UniTask.CompletedTask;
        }
    }

    // Leaving State
    public class LeavingState : CustomerState
    {
        private readonly bool wasSatisfied;

        public LeavingState(Customer customer, bool wasSatisfied) : base(customer, CustomerStateType.Leaving)
        {
            this.wasSatisfied = wasSatisfied;
        }

        public override async UniTask<CustomerState> Enter(CustomerState previousState)
        {
            customer.SetCurrentStateType(CustomerStateType.Leaving);
            customer.LeaveRestaurant(wasSatisfied);

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            return null; // 상태 머신 종료
        }

        public override async UniTask Exit(CustomerState nextState)
        {
            await UniTask.CompletedTask;
        }
    }

    // Customer Main Class
    public class Customer : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private string customerName;
        [SerializeField] private float patience = 100f;
        [SerializeField] private float maxPatience = 100f;
        [SerializeField] private float satisfactionLevel = 0f;
        [SerializeField] private CustomerStateType currentStateType = CustomerStateType.Waiting;
        [SerializeField] private float patienceDecayRate = 1f;

        private StateMachine stateMachine;
        private bool isPatienceDecaying = false;

        public string CustomerName => customerName;
        public float Patience => patience;
        public float MaxPatience => maxPatience;
        public float SatisfactionLevel => satisfactionLevel;
        public CustomerStateType CurrentStateType => currentStateType;

        private async void Start()
        {
            customerName = GenerateRandomName();
            this.Emit(new CustomerSpawnedEvent(this, transform.position));

            // 상태 머신 시작
            stateMachine = new StateMachine(true);
            await stateMachine.Execute<CustomerState>(new WaitingState(this));
        }

        private void Update()
        {
            if (isPatienceDecaying)
            {
                DecayPatience();
            }
        }

        public void StartPatienceDecay()
        {
            isPatienceDecaying = true;
        }

        public void StopPatienceDecay()
        {
            isPatienceDecaying = false;
        }

        private void DecayPatience()
        {
            patience = Mathf.Max(0f, patience - patienceDecayRate * Time.deltaTime);
        }

        public void SetCurrentStateType(CustomerStateType stateType)
        {
            currentStateType = stateType;
        }

        public void CompleteOrder(float serviceQuality = 1f)
        {
            // Calculate satisfaction based on patience remaining and service quality
            float patienceRatio = patience / maxPatience;
            satisfactionLevel = (patienceRatio * 0.6f + serviceQuality * 0.4f) * 100f;

            this.Emit(new CustomerOrderCompletedEvent(this, satisfactionLevel));
        }

        public void LeaveRestaurant(bool wasSatisfied)
        {
            this.Emit(new CustomerLeftEvent(this, wasSatisfied));

            // Destroy customer after a short delay
            Destroy(gameObject, 1f);
        }

        private string GenerateRandomName()
        {
            string[] firstNames = { "김철수", "이영희", "박민수", "최지현", "정수진", "강호동", "송지효", "유재석" };
            return firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
        }

        public void SetCustomerName(string name)
        {
            customerName = name;
        }

        public void SetPatience(float newPatience)
        {
            patience = Mathf.Clamp(newPatience, 0f, maxPatience);
        }

        public void BoostPatience(float amount)
        {
            patience = Mathf.Clamp(patience + amount, 0f, maxPatience);
        }
    }
}