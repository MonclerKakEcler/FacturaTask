using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using Factura.Health;

namespace Factura.Car
{
    public interface ICarController
    {
        void Init(ICarView view);
        Vector3 GetCarPosition();
        void SetCarOnStart();
        void StartMoving();
        void StopMoving();
    }

    public class CarController : ICarController
    {
        private const int kDamageFromEnemy = 50;

        private ICarView _view;
        private CarModel _model;
        private IBaseHealth _health;

        private bool _isMoving = false;
        private Vector3 _initialPosition;
        private Transform _carTransform;

        private CancellationTokenSource _cts;

        public CarController(CarModel model)
        {
            _model = model;
        }

        public void Init(ICarView view)
        {
            _view = view;

            _carTransform = _view.CarTransform;
            _health = _view.Health;
            _initialPosition = _carTransform.position;

            _view.OnHitByEnemy += HandleHitByEnemy;
            _health.OnDied += Die;
        }

        public Vector3 GetCarPosition()
        {
            return _view.CurrentPosition;
        }

        public void SetCarOnStart()
        {
            _carTransform.position = _initialPosition;
        }

        public void StartMoving()
        {
            if (_isMoving) return;

            _cts = new CancellationTokenSource();
            _isMoving = true;

            _health.IsActiveSlider(true);
            _health.ResetHealth();
            _ = MoveLoop(_cts.Token);
        }

        public void StopMoving()
        {
            if (!_isMoving) return;

            _isMoving = false;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _health.IsActiveSlider(false);
        }

        private void HandleHitByEnemy()
        {
            _health.TakeDamage(kDamageFromEnemy);
        }

        private async UniTask MoveLoop(CancellationToken token)
        {
            _carTransform.position = _initialPosition;

            while (!token.IsCancellationRequested)
            {
                _carTransform.Translate(Vector3.forward * _model.Speed * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private void Die()
        {
            StopMoving();
            _view.SetActiveLosePanel(true);

            Time.timeScale = 0;
        }
    }
}