using Data.DataSource;
using Data.Repository;
using Feature.Search.Controller;
using Feature.Search.View;
using UnityEngine;

namespace Feature.Search
{
    public class SearchCompositionRoot : MonoBehaviour
    {
        [SerializeField] private SearchController searchController;
        [SerializeField] private ImageDataSourceAsset imageDataSource;
        
        private IImageRepository _imageRepository;

        private void Awake()
        {
            Debug.Assert(searchController != null, "SearchController 참조가 필요합니다.");
            Debug.Assert(imageDataSource != null, "ImageDataSource 자산이 필요합니다.");

            _imageRepository = new ImageRepository(imageDataSource);
            searchController.Initialize(_imageRepository);
        }
    }
}