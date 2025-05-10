using System;
using UnityEngine;
using Factura.Car;
using Factura.Health;
using Cysharp.Threading.Tasks;

namespace Factura.Enemy
{
    public class EnemyController
    {
        private const int kBulletDamage = 50;
        private const string kIsRunning = "IsRunning";

        private EnemyView _view;
        private EnemyModel _model;
        private EnemyPool _enemyPool;

        private Animator _animator;
        private IBaseHealth _health;
        private ICarController _carController;

        public EnemyController(ICarController carController)
        {
            _carController = carController;
        }

        public void Init(EnemyView view, EnemyModel model, EnemyPool enemyPool)
        {
            _view = view;
            _model = model;
            _enemyPool = enemyPool;

            _animator = _view.Animator;
            _health = _view.Health;

            _view.OnBulletHit += HandleBulletHit;
            _view.OnCarHit += HandleCarHit;
            _health.OnDied += Die;

            _view.RunToCar += Chase;
        }

        public void ResetModel(EnemyModel model)
        {
            _model = model;
        }

        private void Chase()
        {
            Vector3 carPos = _carController.GetCarPosition();
            float distance = Vector3.Distance(_view.EnemyTansform.position, carPos);

            if (distance <= _model.DetectionRadius)
            {
                _view.LookAtSmooth(carPos, 5f);
                _view.MoveTowards(carPos, _model.Speed);
                _animator.SetBool(kIsRunning, true);
            }
            else
            {
                _animator.SetBool(kIsRunning, false);
            }
        }

        private void HandleBulletHit()
        {
            _health.TakeDamage(kBulletDamage);
        }

        private void HandleCarHit()
        {
            _enemyPool.ReturnToPool(_view);
        }

        private void Die()
        {
            _view.OnBulletHit -= HandleBulletHit;
            _view.OnCarHit -= HandleCarHit;
            _health.OnDied -= Die;

            _enemyPool.ReturnToPool(_view);
        }
    }
}
