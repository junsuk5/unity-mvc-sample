using Common.EventSystem;
using Common.Service;
using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Search.View
{
    public struct OnImageClickEvent : IEvent
    {
        public ImageDto ImageData { get; }

        public OnImageClickEvent(ImageDto imageData)
        {
            ImageData = imageData;
        }
    }

    public class ImageGridItem : MonoBehaviour, IMonoEventDispatcher
    {
        private RawImage _imageDisplay;
        private Button _imageButton;
        // [SerializeField] private TextMeshProUGUI tagsText;
        // [SerializeField] private TextMeshProUGUI statsText;

        private ImageDto _imageData;

        private void Awake()
        {
            _imageDisplay = GetComponentInChildren<RawImage>();
            _imageButton = GetComponentInChildren<Button>();
        }

        public async UniTask Initialize(ImageDto imageDto, IImageDownloader downloader)
        {
            _imageData = imageDto;

            // 메타데이터 설정
            // tagsText.text = imageDto.tags;
            // statsText.text = $"❤️ {imageDto.likes} 👁️ {imageDto.views}";

            // 이미지 로딩
            var texture = await downloader.DownloadImageAsync(imageDto.previewURL);
            if (texture != null)
            {
                _imageDisplay.texture = texture;
                _imageDisplay.SetNativeSize();
            }

            // 클릭 이벤트
            _imageButton.onClick.AddListener(OnImageClick);
        }

        private void OnImageClick()
        {
            Debug.Log($"Image clicked: {_imageData.id}");
            // 상세 보기 이벤트 발생 등
            this.Emit(new OnImageClickEvent(_imageData));
        }
    }
}