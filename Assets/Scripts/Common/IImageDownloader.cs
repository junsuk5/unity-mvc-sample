using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    public interface IImageDownloader
    {
        public UniTask<Texture2D> DownloadImageAsync(string url);
    }
}