using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.DataSource;
using Data.Dto;
using Data.Mapper;

namespace Data.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IImageDataSource _imageDataSource;

        public ImageRepository(IImageDataSource imageDataSource)
        {
            _imageDataSource = imageDataSource;
        }

        public async UniTask<List<ImageDto>> GetImageAsync(string query)
        {
            var response = await _imageDataSource.GetImageAsync(query);
            return response.Body.JsonToImageResultDto().ToImageDTOs();
        }
    }
}