using Cysharp.Threading.Tasks;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.Repository
{
    public interface IHomeInfoRepository
    {
        UniTask<HomeInfo> GetHomeInfoAsync();
        IUniTaskAsyncEnumerable<HomeInfo> GetHomeInfoStreamAsync();
    }
}