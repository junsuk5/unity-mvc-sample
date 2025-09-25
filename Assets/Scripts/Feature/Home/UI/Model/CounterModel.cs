using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Feature.Home.Data.Model;
using Feature.Home.Data.Repository;
using UnityEngine;

namespace Feature.Home.UI.Model
{
    public class CounterModel
    {
        public event Action<HomeInfo> OnHomeInfoChanged;

        private IHomeInfoRepository _homeInfoRepository;

        // Stream cancel token
        private CancellationTokenSource _getHomeInfoStreamCancelToken;

        public int Count { get; private set; }

        public CounterModel(
            int count,
            IHomeInfoRepository homeInfoRepository)
        {
            _homeInfoRepository = homeInfoRepository;
            Count = count;
        }

        public void Increment()
        {
            Count++;
        }

        public async void FetchHomeInfoStream()
        {
            _getHomeInfoStreamCancelToken = new CancellationTokenSource();
            await foreach (var result in _homeInfoRepository.GetHomeInfoStreamAsync()
                               .WithCancellation(_getHomeInfoStreamCancelToken.Token))
            {
                if (result.IsSuccess)
                {
                    OnHomeInfoChanged?.Invoke(result.Value);
                }
                else
                {
                    Debug.LogError("GetHomeInfoStreamAsync Error:");
                }
            }
        }
    }
}