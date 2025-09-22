using System;
using UnityEngine;
using Common.EventSystem;

namespace Feature.State.View
{
    public struct PlayerDamagedEvent : IEvent
    {
        public int Damage { get; }

        public PlayerDamagedEvent(int damage)
        {
            Damage = damage;
        }
    }

    public struct PlayerGroundContactEvent : IEvent
    {
    }
    // 애니메이션, 사운드 재생 등 시각적 표현
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerView : MonoBehaviour, IMonoEventDispatcher
    {
        private SpriteRenderer _spriteRenderer;
        private bool _isFlipped = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayJumpAnimation()
        {
            // 점프 시 좌우 반전
            _isFlipped = !_isFlipped;
            _spriteRenderer.flipX = _isFlipped;
            Debug.Log("플레이어 점프 - 스프라이트 반전");
        }

        public void PlayIdleAnimation()
        {
            // 아이들 상태로 복귀 - 기본 값으로 리셋
            _isFlipped = false;
            _spriteRenderer.flipX = _isFlipped;
            Debug.Log("플레이어 아이들 애니메이션 재생 - 기본 방향으로 리셋");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                this.Emit(new PlayerDamagedEvent(damage: 1));
                Destroy(other.gameObject);
            }

            // Ground Layer 와 충돌 체크
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                this.Emit(new PlayerGroundContactEvent());
            }
        }
    }
}