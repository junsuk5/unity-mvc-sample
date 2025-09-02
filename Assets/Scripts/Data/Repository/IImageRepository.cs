using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Dto;

namespace Data.Repository
{
    public interface IImageRepository
    {
        public UniTask<List<ImageDto>> GetImageAsync(string query);
    }
}