using System.Net.Http;
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
        [SerializeField] private bool useMock;

        private IImageRepository _imageRepository;

        private void Awake()
        {
            Debug.Assert(searchController != null, "SearchController 참조가 필요합니다.");

            _imageRepository = new ImageRepository(useMock ? new MockImageDataSource() : new PixabayImageDataSource());
            searchController.Initialize(_imageRepository);
        }
    }
}