using System;

namespace Feature.Home.Model
{
    public class CounterModel
    {
        public event Action<int> OnCountChanged;
        public int Count { get; private set; }

        public CounterModel(int count, Action<int> onCountChanged)
        {
            Count = count;
            OnCountChanged = onCountChanged;
        }

        public void Increment()
        {
            Count++;
            OnCountChanged?.Invoke(Count);
        }
    }
}