using Common.EventSystem;
using UnityEngine;

namespace Feature.State.Controller
{
    public struct PlayerJumpEvent : IEvent
    {
    }

    // Input 을 처리하는 클래스
    public class PlayerInputHandler : MonoBehaviour, IMonoEventDispatcher
    {
        private void Update()
        {
            // 스페이스바 누름 감지
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.Emit<PlayerJumpEvent>();
            }
        }
    }
}