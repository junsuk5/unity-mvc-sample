using Common.EventSystem;
using Common.Routes;
using Feature.Home.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Feature.Home.Controller
{
    public class HomeController : MonoBehaviour, IMonoEventListener
    {
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
            }

            return EventChain.Continue;
        }
    }
}