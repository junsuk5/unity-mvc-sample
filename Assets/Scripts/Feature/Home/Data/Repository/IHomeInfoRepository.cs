using Cysharp.Threading.Tasks;
using Common;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.Repository
{
    public interface IHomeInfoRepository
    {
        UniTask<Result<HomeInfo>> GetHomeInfoAsync();
        IUniTaskAsyncEnumerable<Result<HomeInfo>> GetHomeInfoStreamAsync();
    }
}