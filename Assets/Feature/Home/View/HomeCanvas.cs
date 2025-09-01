using Common.EventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Home.View
{
    public struct OnClickStartGameEvent : IEvent
    {
    }

    public class HomeCanvas : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private Button playButton;

        private void Awake()
        {
            if (playButton != null)
            {
                playButton.onClick.AddListener(this.Emit<OnClickStartGameEvent>);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}