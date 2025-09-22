using Common.EventSystem;
using Common.Routes;
using Feature.Home.Model;
using Feature.Home.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.Home.Controller
{
    public class HomeController : MonoBehaviour, IMonoEventListener
    {
        [SerializeField] private HomeCanvas _view;
        private CounterModel _model;

        private void Awake()
        {
            Debug.Assert(_view != null, "HomeCanvas 참조가 필요합니다.");

            _model = new CounterModel(0, UpdateUI);
        }

        private void UpdateUI(int count)
        {
            _view.UpdateCount(count);
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
            }

            return EventChain.Continue;
        }
    }
}