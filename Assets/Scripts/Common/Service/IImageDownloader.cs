using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Service
{
    public interface IImageDownloader
    {
        public UniTask<Texture2D> DownloadImageAsync(string url);
    }
}