using System;
using Common.EventSystem;
using Cysharp.Threading.Tasks;
using Data.Repository;
using Feature.Search.View;
using UnityEngine;

namespace Feature.Search.Controller
{
    public class SearchController : MonoBehaviour, IMonoEventListener
    {
        [SerializeField] private ImageGridView imageGridView;

        private IImageRepository _imageRepository;

        public void Initialize(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public EventChain OnEventHandle(IEvent param)
        {
            if (param is OnClickSearchButton searchEvent)
            {
                var searchText = searchEvent.SearchText;
                Debug.Log($"SearchText: {searchText}");
                SearchImagesAsync(searchText).Forget();
            }

            return EventChain.Break;
        }


        private async UniTaskVoid SearchImagesAsync(string query)
        {
            try
            {
                var images = await _imageRepository.GetImageAsync(query);
                await imageGridView.DisplayImages(images);
            }
            catch (Exception e)
            {
                Debug.LogError($"Search failed: {e.Message}");
            }
        }
    }
}