using Cysharp.Threading.Tasks;
using Feature.Home.Data.DataSource;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.Repository
{
    public class HomeInfoRepository : IHomeInfoRepository
    {
        private IHomeDataSource<HomeInfo> _homeDataSource;

        public HomeInfoRepository(IHomeDataSource<HomeInfo> homeDataSource)
        {
            _homeDataSource = homeDataSource;
        }

        public UniTask<HomeInfo> GetHomeInfoAsync()
        {
            return _homeDataSource.GetHomeInfoAsync();
        }

        public IUniTaskAsyncEnumerable<HomeInfo> GetHomeInfoStreamAsync()
        {
            return _homeDataSource.GetHomeInfoStreamAsync();
        }
    }
}