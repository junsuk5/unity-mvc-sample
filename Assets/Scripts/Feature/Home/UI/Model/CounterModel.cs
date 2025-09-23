using System;

namespace Feature.Home.UI.Model
{
    public class CounterModel
    {
        public event Action<int> OnCountChanged;
        public int Count { get; private set; }

        public CounterModel(int count)
        {
            Count = count;
        }

        public void Increment()
        {
            Count++;
        }
    }
}