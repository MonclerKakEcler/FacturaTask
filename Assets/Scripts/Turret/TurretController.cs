using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Factura.Turret
{
    public interface ITurretController
    {
        void Init(ITurretView view);
    }

    public class TurretController : ITurretController
    {
        private const int kBulletShootDelayMs = 500;
        private const float _rotationSpeed = 0.2f;

        private readonly IAttackService _attackService;

        private ITurretView _view;

        private bool _isShooting = false;
        private Transform _turretTransform;
        private Transform _firePlaceTransform;
        private ParticleSystem _smokePS;

        public TurretController(IAttackService attackService)
        {
            _attackService = attackService;
        }

        public void Init(ITurretView view)
        {
            _view = view;
            _turretTransform = _view.TurretTransform;
            _firePlaceTransform = _view.FirePlaceTransform;
            _smokePS = _view.SmokePS;

            _view.OnTurretRotateInput += RotateTurret;
            _view.OnShootStart += StartShooting;
            _view.OnShootStop += StopShooting;
        }

        private async void StartShooting()
        {
            if (_isShooting)
            {
                return;
            }

            _isShooting = true;
            await FireLoopAsync();
        }

        private void StopShooting()
        {
            _isShooting = false;
        }

        private void RotateTurret(float deltaX)
        {
            _turretTransform.Rotate(Vector3.up, deltaX * _rotationSpeed, Space.World);
        }

        private async UniTask FireLoopAsync()
        {
            while (_isShooting)
            {
                _smokePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _smokePS.Play();
                _attackService.ProccesAttack(_firePlaceTransform.position, _firePlaceTransform.rotation, _firePlaceTransform.forward);
                await UniTask.Delay(kBulletShootDelayMs);
            }
        }
    }
}
