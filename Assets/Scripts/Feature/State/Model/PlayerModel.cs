using System;
using Common.EventSystem;
using UnityEngine;

namespace Feature.State.Model
{
    public struct PlayerHealthChangedEvent : IEvent
    {
        public int Current { get; }
        public int Max { get; }

        public PlayerHealthChangedEvent(int current, int max)
        {
            Current = current;
            Max = max;
        }
    }

    public struct PlayerDiedEvent : IEvent
    {
    }

// 데이터와 상태를 보관하는 클래스 (체력/위치 등 플레이어 상태)
    public class PlayerModel : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
            this.Emit(new PlayerHealthChangedEvent(currentHealth, maxHealth));
        }

        // 공격 당함
        public void ApplyDamage(int amount)
        {
            var next = Mathf.Max(0, currentHealth - amount);
            if (next == currentHealth) return;

            currentHealth = next;
            this.Emit(new PlayerHealthChangedEvent(currentHealth, maxHealth));

            if (currentHealth == 0)
            {
                this.Emit<PlayerDiedEvent>();
            }
        }
    }
}