using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine.Networking;

namespace Data.DataSource
{
    public class PixabayImageDataSource : IImageDataSource
    {
        private const string BASE_URL = "https://pixabay.com/api/";
        private const string API_KEY = "10711147-dc41758b93b263957026bdadb";

        public async UniTask<ImageResponse> GetImageAsync(string query)
        {
            var request = UnityWebRequest.Get($"{BASE_URL}?key={API_KEY}&q={query}&image_type=photo");
            await request.SendWebRequest();

            return new ImageResponse(
                statusCode: (int)request.responseCode,
                headers: request.GetResponseHeaders(),
                body: request.downloadHandler.text
            );
        }
    }
}