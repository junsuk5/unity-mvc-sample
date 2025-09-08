using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Search.View
{
    public class ImageGridView : MonoBehaviour
    {
        [SerializeField] private ImageGridItem itemPrefab;

        private ScrollRect _scrollRect;

        private readonly List<ImageGridItem> _activeItems = new();
        private IImageDownloader _imageDownloader;

        private void Awake()
        {
            _imageDownloader = new UnityWebRequestImageDownloader();
            _scrollRect = GetComponentInParent<ScrollRect>();
        }

        public async UniTask DisplayImages(List<ImageDto> images)
        {
            ClearGrid();

            foreach (var imageDto in images)
            {
                var item = Instantiate(itemPrefab, transform);
                await item.Initialize(imageDto, _imageDownloader);
                _activeItems.Add(item);
            }

            // 스크롤을 맨 위로
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        private void ClearGrid()
        {
            foreach (var item in _activeItems)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }

            _activeItems.Clear();
        }
    }
}