using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Data.Dto;

namespace Data.DataSource
{
    public interface IImageDataSource
    {
        UniTask<ImageResponse> GetImageAsync(string query);
    }
}