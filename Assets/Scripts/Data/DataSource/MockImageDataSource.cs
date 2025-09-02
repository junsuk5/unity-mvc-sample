using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Dto;
using UnityEngine;

namespace Data.DataSource
{
    public class MockImageDataSource: IImageDataSource
    {
        public async UniTask<ImageResponse> GetImageAsync(string query)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>("mockdata");
            
            if (jsonTextAsset == null)
            {
                throw new System.IO.FileNotFoundException($"Mock JSON file 'mockdata.json' not found in Resources folder.");
            }

            string jsonString = jsonTextAsset.text;
        
            // 목 데이터는 항상 성공적인 응답을 반환한다고 가정
            var mockResponse = new ImageResponse(
                statusCode: 200,
                headers: new Dictionary<string, string>(),
                body: jsonString
            );

            return await UniTask.FromResult(mockResponse);
        }
    }
}