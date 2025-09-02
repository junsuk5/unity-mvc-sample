# 🎯 Unity StateMachine 시스템 교육 가이드

## 📚 목차

1. [개념 이해](#개념-이해)
2. [기본 구조](#기본-구조)
3. [핵심 인터페이스](#핵심-인터페이스)
4. [실습 예제](#실습-예제)
5. [고급 활용](#고급-활용)
6. [문제 해결](#문제-해결)

---

## 🧠 개념 이해

### StateMachine이란?

상태 머신은 **객체의 상태를 체계적으로 관리하는 디자인 패턴**입니다.

### 왜 StateMachine을 사용할까?

- ✅ **복잡한 로직 단순화**: 상태별로 로직을 분리
- ✅ **버그 감소**: 상태 전환 로직을 명확하게 정의
- ✅ **유지보수성**: 새로운 상태 추가가 쉬움
- ✅ **가독성**: 코드의 의도가 명확해짐

### 실생활 예시

```
🚦 신호등 시스템
- 빨간불 → 노란불 → 초록불 → 노란불 → 빨간불
- 각 상태에서 다른 동작 수행
```

---

## 🏗️ 기본 구조

### 1. 핵심 인터페이스

```csharp
public interface IState<T>
{
    UniTask<T> Enter(T previousState);  // 상태 진입
    UniTask Exit(T nextState);          // 상태 종료
}
```

### 2. StateMachine 클래스

```csharp
public class StateMachine
{
    public async UniTask Execute<T>(T state) where T : class, IState<T>
    {
        // 상태를 순차적으로 실행
        // null을 반환할 때까지 계속
    }
}
```

---

## 🔧 핵심 인터페이스 상세

### IState<T> 인터페이스

```csharp
public interface IState<T>
{
    // 상태에 진입할 때 호출
    // 다음 상태를 반환 (null이면 종료)
    UniTask<T> Enter(T previousState);

    // 상태를 나갈 때 호출
    // 다음 상태로 전환하기 전 정리 작업
    UniTask Exit(T nextState);
}
```

### StateMachine 클래스

```csharp
public class StateMachine
{
    private readonly bool isLogging;

    public StateMachine(bool isLogging)
    {
        this.isLogging = isLogging;
    }

    public async UniTask Execute<T>(T state) where T : class, IState<T>
    {
        T previousState = null;
        T currentState = state;

        while (currentState != null)
        {
            // 이전 상태 종료
            if (previousState != null)
            {
                await previousState.Exit(currentState);
            }

            // 로깅
            if (isLogging)
            {
                Debug.Log($"새로운 상태 진입 : {currentState.GetType().Name}");
            }

            // 현재 상태 진입 및 다음 상태 결정
            var nextState = await currentState.Enter(previousState);
            previousState = currentState;
            currentState = nextState;
        }
    }
}
```

---

## 🎮 실습 예제

### 예제 1: 간단한 카운터 상태 머신

```csharp
using Cysharp.Threading.Tasks;
using UnityEngine;
using Common.StatementSystem;

namespace Examples
{
    // 카운터 상태 인터페이스
    public class CounterState : IState<CounterState>
    {
        public virtual UniTask<CounterState> Enter(CounterState previousState)
            => UniTask.FromResult<CounterState>(null);
        public virtual UniTask Exit(CounterState nextState) => UniTask.CompletedTask;
    }

    // 시작 상태
    public class StartState : CounterState
    {
        public override async UniTask<CounterState> Enter(CounterState previousState)
        {
            Debug.Log("카운터 시작!");
            await UniTask.Delay(1000);
            return new CountState(); // 다음 상태로 이동
        }
    }

    // 카운트 상태
    public class CountState : CounterState
    {
        private int count = 0;
        private const int MAX_COUNT = 5;

        public override async UniTask<CounterState> Enter(CounterState previousState)
        {
            count++;
            Debug.Log($"카운트: {count}");
            await UniTask.Delay(500);

            if (count >= MAX_COUNT)
                return new EndState(); // 종료 상태로
            else
                return new CountState(); // 계속 카운트
        }
    }

    // 종료 상태
    public class EndState : CounterState
    {
        public override async UniTask<CounterState> Enter(CounterState previousState)
        {
            Debug.Log("카운터 종료!");
            await UniTask.Delay(1000);
            return null; // 상태 머신 종료
        }
    }

    // 사용 예시
    public class CounterExample : MonoBehaviour
    {
        private void Start()
        {
            var stateMachine = new StateMachine(true);
            stateMachine.Execute(new StartState());
        }
    }
}
```

### 예제 2: 플레이어 상태 머신

```csharp
public class PlayerState : IState<PlayerState>
{
    protected PlayerController player;

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    public virtual UniTask<PlayerState> Enter(PlayerState previousState)
        => UniTask.FromResult<PlayerState>(null);
    public virtual UniTask Exit(PlayerState nextState) => UniTask.CompletedTask;
}

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player) { }

    public override async UniTask<PlayerState> Enter(PlayerState previousState)
    {
        Debug.Log("플레이어 대기 상태");
        player.animator.SetTrigger("Idle");

        // 입력 대기
        while (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.Space))
        {
            await UniTask.Yield();
        }

        if (Input.GetKeyDown(KeyCode.W))
            return new WalkState(player);
        else
            return new JumpState(player);
    }
}

public class WalkState : PlayerState
{
    public WalkState(PlayerController player) : base(player) { }

    public override async UniTask<PlayerState> Enter(PlayerState previousState)
    {
        Debug.Log("플레이어 걷기 상태");
        player.animator.SetTrigger("Walk");

        float walkTime = 0f;
        while (walkTime < 3f && Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * Time.deltaTime * 5f);
            walkTime += Time.deltaTime;
            await UniTask.Yield();
        }

        return new IdleState(player);
    }
}
```

---

## 🚀 고급 활용

### 1. 상태 데이터 공유

```csharp
public class GameStateData
{
    public int score = 0;
    public float time = 0f;
    public bool isGameOver = false;
}

public class GameState : IState<GameState>
{
    protected GameStateData data;

    public GameState(GameStateData data)
    {
        this.data = data;
    }

    // ... 상태 구현
}
```

### 2. 조건부 상태 전환

```csharp
public class SmartState : PlayerState
{
    public override async UniTask<PlayerState> Enter(PlayerState previousState)
    {
        // 여러 조건을 확인하여 상태 결정
        if (player.health <= 0)
            return new DeathState(player);
        else if (player.isAttacking)
            return new AttackState(player);
        else if (Input.GetKey(KeyCode.W))
            return new WalkState(player);
        else
            return new IdleState(player);
    }
}
```

### 3. 상태 히스토리 관리

```csharp
public class StateWithHistory : PlayerState
{
    private Stack<PlayerState> stateHistory = new Stack<PlayerState>();

    public override async UniTask<PlayerState> Enter(PlayerState previousState)
    {
        if (previousState != null)
            stateHistory.Push(previousState);

        // 뒤로가기 기능
        if (Input.GetKeyDown(KeyCode.Backspace) && stateHistory.Count > 0)
            return stateHistory.Pop();

        return new NextState(player);
    }
}
```

---

## 🔍 문제 해결

### Q1: 상태가 전환되지 않아요

**A:** `Enter` 메서드에서 다음 상태를 반환하고 있는지 확인하세요.

```csharp
// ❌ 잘못된 예시
public override async UniTask<PlayerState> Enter(PlayerState previousState)
{
    Debug.Log("상태 진입");
    // return 문이 없음!
}

// ✅ 올바른 예시
public override async UniTask<PlayerState> Enter(PlayerState previousState)
{
    Debug.Log("상태 진입");
    return new NextState(); // 다음 상태 반환
}
```

### Q2: 상태 머신이 종료되지 않아요

**A:** `null`을 반환하여 상태 머신을 종료하세요.

```csharp
public override async UniTask<PlayerState> Enter(PlayerState previousState)
{
    Debug.Log("최종 상태");
    return null; // 상태 머신 종료
}
```

### Q3: 비동기 작업이 제대로 처리되지 않아요

**A:** `await` 키워드를 올바르게 사용하세요.

```csharp
// ❌ 잘못된 예시
public override async UniTask<PlayerState> Enter(PlayerState previousState)
{
    UniTask.Delay(1000); // await 없음
    return new NextState();
}

// ✅ 올바른 예시
public override async UniTask<PlayerState> Enter(PlayerState previousState)
{
    await UniTask.Delay(1000); // await 사용
    return new NextState();
}
```

---

## 📝 실습 과제

### 과제 1: 간단한 애니메이션 상태 머신

- Idle → Walk → Run → Jump → Idle 순서로 전환
- 각 상태에서 적절한 애니메이션 재생

### 과제 2: 게임 메뉴 상태 머신

- MainMenu → Settings → GamePlay → Pause → MainMenu
- 각 상태에서 적절한 UI 패널 표시

### 과제 3: AI 캐릭터 상태 머신

- Patrol → Chase → Attack → Return → Patrol
- 각 상태에서 적절한 AI 행동 수행

---

## 🎯 핵심 포인트 정리

1. **상태 분리**: 각 상태는 독립적인 로직을 가져야 함
2. **명확한 전환**: 상태 간 전환 조건을 명확히 정의
3. **비동기 처리**: UniTask를 활용한 비동기 상태 관리
4. **데이터 공유**: 상태 간 필요한 데이터를 적절히 공유
5. **종료 조건**: null 반환으로 상태 머신 종료

---

## 🔗 추가 학습 자료

- [Unity UniTask 공식 문서](https://github.com/Cysharp/UniTask)
- [State Pattern 디자인 패턴](https://refactoring.guru/design-patterns/state)
- [Unity 코루틴 vs UniTask 비교](https://medium.com/@cysharp/unitask-vs-coroutine-which-one-should-you-use-9b6de2836b8f)

---

_이 가이드를 통해 StateMachine의 개념과 활용법을 마스터하세요! 🚀_
