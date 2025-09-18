using Common.EventSystem;
using UnityEngine;

namespace Feature.State.Controller
{
    public struct PlayerDamagedEvent : IEvent
    {
        public int Damage { get; }

        public PlayerDamagedEvent(int damage)
        {
            Damage = damage;
        }
    }

    public class PlayerHitDetector : MonoBehaviour, IMonoEventDispatcher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                this.Emit(new PlayerDamagedEvent(damage: 1));
                Destroy(other.gameObject);
            }
        }
    }
}