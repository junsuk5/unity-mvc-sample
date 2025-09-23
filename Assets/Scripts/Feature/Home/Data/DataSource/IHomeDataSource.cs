using Cysharp.Threading.Tasks;
using Feature.Home.Data.Model;

namespace Feature.Home.Data.DataSource
{
    public interface IHomeDataSource<T>
    {
        public UniTask<T> GetHomeInfoAsync();
        public IUniTaskAsyncEnumerable<T> GetHomeInfoStreamAsync();
    }
}