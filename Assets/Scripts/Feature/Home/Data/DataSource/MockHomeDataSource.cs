using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.DataSource
{
    public class MockHomeDataSource : IHomeDataSource<Dictionary<string, object>>
    {
        public UniTask<Dictionary<string, object>> GetHomeInfoAsync()
        {
            return UniTask.FromResult(
                new Dictionary<string, object>
                {
                    {
                        "imageUrl",
                        "https://www.news1.kr/_next/image?url=https%3A%2F%2Fi3n.news1.kr%2Fsystem%2Fphotos%2F2023%2F4%2F28%2F5966469%2Fhigh.jpg&w=1920&q=75"
                    },
                    { "searchString", "아이유" },
                    { "count", 100 }
                }
            );
        }

        public IUniTaskAsyncEnumerable<Dictionary<string, object>> GetHomeInfoStreamAsync()
        {
            return UniTaskAsyncEnumerable.Create<Dictionary<string, object>>(async (writer, token) =>
            {
                var samples = new[]
                {
                    new Dictionary<string, object>
                    {
                        {
                            "imageUrl",
                            "https://file2.nocutnews.co.kr/newsroom/image/2025/05/05/202505052121046469_0.jpg"
                        },
                        { "searchString", "아이유 서울 콘서트" },
                        { "count", 1287 }
                    },
                    new Dictionary<string, object>
                    {
                        {
                            "imageUrl",
                            "https://newsimg.hankookilbo.com/cms/articlerelease/2021/05/17/b41ab909-e0e2-40e8-a36a-4bae809a9024.jpg"
                        },
                        { "searchString", "아이유 새 앨범 예약" },
                        { "count", 3421 }
                    },
                    new Dictionary<string, object>
                    {
                        {
                            "imageUrl",
                            "https://file2.nocutnews.co.kr/newsroom/image/2025/09/10/202509101111360528_0.jpg"
                        },
                        { "searchString", "팬클럽 신규 가입자" },
                        { "count", 956 }
                    }
                };

                foreach (var sample in samples)
                {
                    await writer.YieldAsync(sample);
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                }
            });
        }
    }
}