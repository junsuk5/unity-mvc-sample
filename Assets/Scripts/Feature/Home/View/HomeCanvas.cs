using Common.EventSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Home.View
{
    public struct OnClickStartGameEvent : IEvent
    {
    }

    public struct OnClickImageSearchEvent : IEvent
    {
    }

    public class HomeCanvas : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private Button playButton;
        [SerializeField] private CanvasGroup buttonCanvasGroup;

        [SerializeField] private Button imageSearchButton;

        private void Awake()
        {
            // 버튼을 처음에 투명하게 설정
            if (buttonCanvasGroup != null)
            {
                buttonCanvasGroup.alpha = 0f;
                buttonCanvasGroup.interactable = false;
            }

            if (playButton != null)
            {
                playButton.onClick.AddListener(this.Emit<OnClickStartGameEvent>);
            }

            if (imageSearchButton != null)
            {
                imageSearchButton.onClick.AddListener(this.Emit<OnClickImageSearchEvent>);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private async void Start()
        {
            // 2초 대기 후 페이드 인 시작
            await UniTask.Delay(2000);
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}