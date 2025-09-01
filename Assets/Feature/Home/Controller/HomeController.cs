using Common.EventSystem;
using Common.Routes;
using Feature.Home.View;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
                // SceneManager.LoadScene("Feature/Play/Play");
                Addressables.LoadSceneAsync(RouteNames.Play);
            }

            return EventChain.Break;
        }
    }
}