using UnityEngine;

namespace Feature.State.View
{
    // 애니메이션, 사운드 재생 등 시각적 표현
    public class PlayerView : MonoBehaviour
    {
        public void PlayJumpAnimation()
        {
            Debug.Log("플레이어 점프 애니메이션 재생");
        }

        public void PlayIdleAnimation()
        {
            Debug.Log("플레이어 걷기 애니메이션 재생");
        }
    }
}