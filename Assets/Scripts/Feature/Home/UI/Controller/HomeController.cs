using System;
using System.Threading;
using Common;
using Common.EventSystem;
using Common.Routes;
using Common.Service;
using Cysharp.Threading.Tasks;
using Data.Repository;
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
        [SerializeField] private HomeCanvas _view;

        private IImageDownloader _downloader;
        private CounterModel _model;

        private void Awake()
        {
            Debug.Assert(_view != null, "HomeCanvas 참조가 필요합니다.");
        }

        public void Initialize(IImageDownloader downloader, CounterModel model)
        {
            _model = model;
            _downloader = downloader;

            _model.OnHomeInfoChanged += UpdateUI;
        }

        private void Start()
        {
            _model.FetchHomeInfoStream();
        }

        private void OnDestroy()
        {
            _model.OnHomeInfoChanged -= UpdateUI;
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