using Cysharp.Threading.Tasks;

namespace Feature.Home.Data.DataSource
{
    public interface IHomeDataSource<T>
    {
        public UniTask<T> GetHomeInfoAsync();
        public IUniTaskAsyncEnumerable<T> GetHomeInfoStreamAsync();
    }
}