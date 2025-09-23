using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.DataSource
{
    public class MockHomeDataSource: IHomeDataSource<HomeInfo>
    {
        public UniTask<HomeInfo> GetHomeInfoAsync()
        {
            return UniTask.FromResult(new HomeInfo("https://www.news1.kr/_next/image?url=https%3A%2F%2Fi3n.news1.kr%2Fsystem%2Fphotos%2F2023%2F4%2F28%2F5966469%2Fhigh.jpg&w=1920&q=75", "아이유", 100));
        }

        public IUniTaskAsyncEnumerable<HomeInfo> GetHomeInfoStreamAsync()
        {
            return UniTaskAsyncEnumerable.Create<HomeInfo>(async (writer, token) =>
            {
                var samples = new[]
                {
                    new HomeInfo(
                        "https://file2.nocutnews.co.kr/newsroom/image/2025/05/05/202505052121046469_0.jpg",
                        "아이유 서울 콘서트",
                        1287),
                    new HomeInfo(
                        "https://newsimg.hankookilbo.com/cms/articlerelease/2021/05/17/b41ab909-e0e2-40e8-a36a-4bae809a9024.jpg",
                        "아이유 새 앨범 예약",
                        3421),
                    new HomeInfo(
                        "https://file2.nocutnews.co.kr/newsroom/image/2025/09/10/202509101111360528_0.jpg",
                        "팬클럽 신규 가입자",
                        956)
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