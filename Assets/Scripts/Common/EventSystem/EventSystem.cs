using UnityEngine;

namespace Common.EventSystem
{
    public interface IEvent
    {
    }
    public enum EventChain
    {
        Continue,
        Break,
    }
    public interface IMonoEventListener
    {
        GameObject gameObject { get; }
        EventChain OnEventHandle(IEvent param);
    }

    public interface IMonoEventDispatcher
    {
        GameObject gameObject { get; }
    }

    public static class MonoEventDispatcherExtension
    {
        public static void Emit(this IMonoEventDispatcher dispatcher, IEvent param)
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;

            var eventListeners = dispatcher.gameObject.GetComponentsInParent<IMonoEventListener>();
            foreach (var eventListener in eventListeners)
            {
                if (eventListener.OnEventHandle(param) == EventChain.Break) return;
            }
        }

        public static void Emit<T>(this IMonoEventDispatcher dispatcher) where T : IEvent, new()
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;

            var eventListeners = dispatcher.gameObject.GetComponentsInParent<IMonoEventListener>();
            foreach (var eventListener in eventListeners)
            {
                if (eventListener.OnEventHandle(new T()) == EventChain.Break) return;
            }
        }
    }
}