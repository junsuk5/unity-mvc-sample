using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Common;
using Feature.Home.Data.DataSource;
using Feature.Home.Data.Mapper;
using Feature.Home.Data.Model;
using UnityEngine;

namespace Feature.Home.Data.Repository
{
    public class HomeInfoRepository : IHomeInfoRepository
    {
        private IHomeDataSource<Dictionary<string, object>> _homeDataSource;

        public HomeInfoRepository(IHomeDataSource<Dictionary<string, object>> homeDataSource)
        {
            _homeDataSource = homeDataSource;
        }

        public async UniTask<Result<HomeInfo>> GetHomeInfoAsync()
        {
            try
            {
                var data = await _homeDataSource.GetHomeInfoAsync();
                return Result<HomeInfo>.Success(data.ToModel());
            }
            catch (Exception ex)
            {
                Debug.LogError($"HomeInfoRepository: Failed to get home info: {ex.Message}");
                return Result<HomeInfo>.Failure($"데이터를 가져오는데 실패했습니다: {ex.Message}");
            }
        }

        public IUniTaskAsyncEnumerable<Result<HomeInfo>> GetHomeInfoStreamAsync()
        {
            try
            {
                return _homeDataSource.GetHomeInfoStreamAsync()
                    .Select(data =>
                    {
                        try
                        {
                            return Result<HomeInfo>.Success(data.ToModel());
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"HomeInfoRepository: Failed to process stream data: {ex.Message}");
                            return Result<HomeInfo>.Failure($"스트림 데이터 처리 실패: {ex.Message}");
                        }
                    });
            }
            catch (Exception ex)
            {
                Debug.LogError($"HomeInfoRepository: Failed to get home info stream: {ex.Message}");
                return UniTaskAsyncEnumerable.Empty<Result<HomeInfo>>();
            }
        }
    }
}