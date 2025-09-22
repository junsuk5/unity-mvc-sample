using Common.EventSystem;
using Cysharp.Threading.Tasks;
using TMPro;
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
    
    public struct OnClickIncreaseEvent : IEvent
    {
    }

    public struct OnClickHelpEvent : IEvent
    {
    }

    public class HomeCanvas : MonoBehaviour, IMonoEventDispatcher
    {
        [SerializeField] private Button playButton;
        [SerializeField] private CanvasGroup buttonCanvasGroup;

        [SerializeField] private Button imageSearchButton;
        [SerializeField] private Button increaseButton;
        [SerializeField] private Button helpButton;

        private void Awake()
        {
            Debug.Assert(playButton != null);
            Debug.Assert(buttonCanvasGroup != null);
            Debug.Assert(imageSearchButton != null);
            Debug.Assert(increaseButton != null);
            Debug.Assert(helpButton != null);

            // 버튼을 처음에 투명하게 설정
            buttonCanvasGroup.alpha = 0f;
            buttonCanvasGroup.interactable = false;

            playButton.onClick.AddListener(this.Emit<OnClickStartGameEvent>);
            imageSearchButton.onClick.AddListener(this.Emit<OnClickImageSearchEvent>);
            increaseButton.onClick.AddListener(this.Emit<OnClickIncreaseEvent>);
            helpButton.onClick.AddListener(this.Emit<OnClickHelpEvent>);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private async void Start()
        {
            // 2초 대기 후 페이드 인 시작
            await UniTask.Delay(2000);
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
        }

        public void UpdateCount(int count)
        {
            increaseButton.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}