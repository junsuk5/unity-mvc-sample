using System.Collections.Generic;
using Common.Service;
using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.Search.View
{
    public class ImageGridView : MonoBehaviour
    {
        private ImageGridItem _itemPrefab;

        [SerializeField] private ScrollRect scrollRect;

        private readonly List<ImageGridItem> _activeItems = new();
        private IImageDownloader _imageDownloader;

        private void Awake()
        {
            _imageDownloader = new UnityWebRequestImageDownloader();
            
            LoadPrefab();
        }
        
        private void LoadPrefab()
        {
            // Resources 폴더에서 기존 프리팹 로드
            var prefabResource = Resources.Load<GameObject>("ImageGridItem");
            
            if (prefabResource != null)
            {
                _itemPrefab = prefabResource.GetComponent<ImageGridItem>();
            }
            else
            {
                Debug.LogError("Failed to load ImageGridItem prefab from Resources.");
            }
        }

        public async UniTask DisplayImages(List<ImageDto> images)
        {
            // null 안전성 확인
            if (_itemPrefab == null)
            {
                Debug.LogError("ItemPrefab is null. Cannot display images.");
                return;
            }

            ClearGrid();

            foreach (var imageDto in images)
            {
                var item = Instantiate(_itemPrefab, scrollRect.content, false);
                // 프리팹에서 생성된 아이템은 이미 활성화 상태
                await item.Initialize(imageDto, _imageDownloader);
                _activeItems.Add(item);
            }

            // 스크롤을 맨 위로
            scrollRect.verticalNormalizedPosition = 1f;
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