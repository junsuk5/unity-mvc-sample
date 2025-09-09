using Common.EventSystem;
using Common.Routes;
using Feature.Home.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.Home.Controller
{
    public class HomeController : MonoBehaviour, IMonoEventListener
    {
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public EventChain OnEventHandle(IEvent param)
        {
            if (param is OnClickStartGameEvent)
            {
                Debug.Log("OnClickStartGameEvent");
                SceneManager.LoadScene(RouteNames.Play);
                // Addressables.LoadSceneAsync(RouteNames.Play);
            }
            else if (param is OnClickImageSearchEvent)
            {
                Debug.Log("OnClickImageSearchEvent");
                SceneManager.LoadScene(RouteNames.Search);
            }

            return EventChain.Break;
        }
    }
}