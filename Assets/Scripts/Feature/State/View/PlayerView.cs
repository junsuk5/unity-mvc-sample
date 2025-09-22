using System;
using UnityEngine;

namespace Feature.State.View
{
    // 애니메이션, 사운드 재생 등 시각적 표현
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Sprite jumpSprite;
        [SerializeField] private Sprite idleSprite;

        private SpriteRenderer _spriteRenderer;


        private void Awake()
        {
            Debug.Assert(jumpSprite != null);
            Debug.Assert(idleSprite != null);
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayJumpAnimation()
        {
            // 점프 스프라이트로 교체
            _spriteRenderer.sprite = jumpSprite;
            Debug.Log("플레이어 점프 애니메이션 재생");
        }

        public void PlayIdleAnimation()
        {
            _spriteRenderer.sprite = idleSprite;
            Debug.Log("플레이어 걷기 애니메이션 재생");
        }
    }
}