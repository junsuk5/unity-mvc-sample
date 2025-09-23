using System;
using System.Threading;
using Common;
using Common.EventSystem;
using Common.Routes;
using Common.Service;
using Cysharp.Threading.Tasks;
using Feature.Home.Data.DataSource;
using Feature.Home.Data.Model;
using Feature.Home.Data.Repository;
using Feature.Home.UI.Model;
using Feature.Home.UI.View;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.Home.UI.Controller
{
    public class HomeController : MonoBehaviour, IMonoEventListener
    {
        // Repository
        private IHomeInfoRepository _homeInfoRepository;

        private IImageDownloader _downloader;

        // Stream cancel token
        private CancellationTokenSource _getHomeInfoStreamCancelToken;

        [SerializeField] private HomeCanvas _view;
        private CounterModel _model;

        private void Awake()
        {
            Debug.Assert(_view != null, "HomeCanvas 참조가 필요합니다.");

            _model = new CounterModel(0);

            // Repository
            // _homeInfoRepository = new HomeInfoRepository(new MockHomeDataSource());
            _homeInfoRepository =
                new HomeInfoRepository(new FirestoreHomeDataSource(FirebaseFirestore.DefaultInstance));
            _downloader = new UnityWebRequestImageDownloader();
        }

        private async void Start()
        {
            _getHomeInfoStreamCancelToken = new CancellationTokenSource();
            await foreach (var result in _homeInfoRepository.GetHomeInfoStreamAsync()
                               .WithCancellation(_getHomeInfoStreamCancelToken.Token))
            {
                if (result.IsSuccess)
                {
                    UpdateUI(result.Value);
                }
                else
                {
                    Debug.LogError("GetHomeInfoStreamAsync Error:");
                }
            }
        }

        private async void UpdateUI(HomeInfo homeInfo)
        {
            _view.UpdateCount(homeInfo.Count);

            if (!string.IsNullOrWhiteSpace(homeInfo.PlayImageUrl))
            {
                var texture2D = await _downloader.DownloadImageAsync(homeInfo.PlayImageUrl);
                _view.UpdateImage(texture2D);
            }
        }

        public EventChain OnEventHandle(IEvent param)
        {
            switch (param)
            {
                case OnClickStartGameEvent:
                    Debug.Log("OnClickStartGameEvent");
                    SceneManager.LoadScene(RouteNames.Play);
                    return EventChain.Break;

                case OnClickImageSearchEvent:
                    Debug.Log("OnClickImageSearchEvent");
                    SceneManager.LoadScene(RouteNames.Search);
                    return EventChain.Break;

                case OnClickIncreaseEvent:
                    Debug.Log("OnClickIncreaseEvent");
                    _model.Increment();
                    return EventChain.Break;

                case OnClickHelpEvent:
                    Debug.Log("OnClickHelpEvent");
                    // TODO: 도움말 화면 표시 로직 구현
                    return EventChain.Break;
            }

            return EventChain.Continue;
        }
    }
}