using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Firebase.Firestore;

namespace Feature.Home.Data.DataSource
{
    public class FirestoreHomeDataSource: IHomeDataSource<Dictionary<string, object>>
    {
        private FirebaseFirestore _db;

        public FirestoreHomeDataSource(FirebaseFirestore db)
        {
            _db = db;
        }

        public UniTask<Dictionary<string, object>> GetHomeInfoAsync()
        {
            throw new NotImplementedException();
        }

        public IUniTaskAsyncEnumerable<Dictionary<string, object>> GetHomeInfoStreamAsync()
        {
            DocumentReference docRef = _db.Collection("home").Document("100");

            // IUniTaskAsyncEnumerable.Create를 사용하여 콜백을 스트림으로 변환
            return UniTaskAsyncEnumerable.Create<Dictionary<string, object>>(async (writer, cancellationToken) =>
            {
                using var listenerCts = new CancellationTokenSource();
                var listenerToken = listenerCts.Token;

                // Firestore 리스너를 등록하고 IDisposable을 얻음
                using var registration = docRef.Listen(snapshot =>
                {
                    // 단순하게 데이터만 변환하고 전달
                    Dictionary<string, object> data = snapshot.ToDictionary();

                    // writer.YieldAsync를 사용하여 데이터를 스트림으로 내보냄
                    UniTask.Void(async () =>
                    {
                        if (cancellationToken.IsCancellationRequested || listenerToken.IsCancellationRequested)
                        {
                            return;
                        }
                        await writer.YieldAsync(data);
                    });
                });

                // 외부 CancellationToken이 취소되면 리스너도 취소
                cancellationToken.Register(() => listenerCts.Cancel());

                // 외부 CancellationToken이 취소되거나 리스너가 제거될 때까지 대기
                await UniTask.WaitUntilCanceled(cancellationToken);
            });
        }
    }
}