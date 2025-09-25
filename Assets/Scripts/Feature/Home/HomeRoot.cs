using System.Collections.Generic;
using Common.Service;
using Feature.Home.Data.DataSource;
using Feature.Home.Data.Repository;
using Feature.Home.UI.Controller;
using Feature.Home.UI.Model;
using Firebase.Firestore;
using UnityEngine;

namespace Feature.Home
{
    // 의존성 조립/주입
    public class HomeRoot : MonoBehaviour
    {
        [SerializeField] private HomeController homeController;
        [SerializeField] private bool useMock;

        private IHomeDataSource<Dictionary<string, object>> _homeDataSource;
        private IHomeInfoRepository _homeInfoRepository;
        private IImageDownloader _downloader;
        private CounterModel _model;

        private void Awake()
        {
            Debug.Assert(homeController != null);
            Debug.Assert(_homeInfoRepository != null);

            if (useMock)
            {
                _homeDataSource = new MockHomeDataSource();
            }
            else
            {
                _homeDataSource = new FirestoreHomeDataSource(FirebaseFirestore.DefaultInstance);
            }

            _homeInfoRepository = new HomeInfoRepository(_homeDataSource);
            _downloader = new UnityWebRequestImageDownloader();

            _model = new CounterModel(0, _homeInfoRepository);

            homeController.Initialize(_downloader, _model);
        }
    }
}