using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine;

namespace Data.DataSource
{
    public abstract class ImageDataSourceAsset : ScriptableObject, IImageDataSource
    {
        public abstract UniTask<ImageResponse> GetImageAsync(string query);
    }
}