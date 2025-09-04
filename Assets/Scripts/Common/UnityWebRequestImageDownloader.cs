using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    public class UnityWebRequestImageDownloader : IImageDownloader
    {
        public async UniTask<Texture2D> DownloadImageAsync(string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            await request.SendWebRequest();
            return DownloadHandlerTexture.GetContent(request);
        }
    }
}