using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Factura.Bullet
{
    public class BulletItem : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private const int kLifeTimeBulletMs = 3000;

        private Action<BulletItem> _returnToPool;
        private CancellationTokenSource _lifeTimeCancellationTokenSource;

        public void Initialize(Action<BulletItem> returnToPool)
        {
            _returnToPool = returnToPool;
        }

        public void Shoot()
        {
            _lifeTimeCancellationTokenSource = new CancellationTokenSource();
            _ = StartLifeTimer(_lifeTimeCancellationTokenSource.Token);
        }

        public void ReturnToPool()
        {
            _lifeTimeCancellationTokenSource?.Cancel();
            _lifeTimeCancellationTokenSource?.Dispose();
            _lifeTimeCancellationTokenSource = null;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _returnToPool?.Invoke(this);
        }

        private async UniTask StartLifeTimer(CancellationToken token)
        {
            await UniTask.Delay(kLifeTimeBulletMs, cancellationToken: token);
            ReturnToPool();
        }
    }
}