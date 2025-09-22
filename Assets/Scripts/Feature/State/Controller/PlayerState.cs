using Common.StatementSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Feature.State.Controller
{
    public abstract class PlayerStateBase : IState<PlayerStateBase>
    {
        public abstract UniTask<PlayerStateBase> Enter(PlayerStateBase previousState);

        public virtual UniTask Exit(PlayerStateBase nextState)
        {
            return UniTask.CompletedTask;
        }
    }

    public class IdleState : PlayerStateBase
    {
        public override UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            return UniTask.FromResult<PlayerStateBase>(null);
        }
    }

    public class JumpState : PlayerStateBase
    {
        private readonly PlayerController _controller;

        public JumpState(PlayerController controller)
        {
            _controller = controller;
        }

        public override async UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            // 점프 애니메이션 등
            _controller.Jump();
            await UniTask.WaitUntil(() => _controller.IsGrounded);
            return new IdleState();
        }
    }
    
    // 죽음 상태
    public class DiedState : PlayerStateBase
    {
        private readonly PlayerController _controller;

        public DiedState(PlayerController controller)
        {
            _controller = controller;
        }

        public override UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            Debug.Log("사망");
            return UniTask.FromResult<PlayerStateBase>(new IdleState());
        }
    }
}
