using UnityEngine;
using Zenject;
using Factura.Car;
using Factura.Enemy;
using Factura.Turret;
using Factura.Bullet;

namespace Factura.Core
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private BulletItem _bulletPrefab;
        [SerializeField] private EnemyView _enemyPrefab;

        public override void InstallBindings()
        {
            BindEnemy();
            BindBullet();
            BindCar();
            BindTurret();
        }

        private void BindEnemy()
        {
            Container.Bind<EnemyView>().FromInstance(_enemyPrefab).AsSingle();
            Container.Bind<EnemyController>().To<EnemyController>().AsTransient();
            Container.Bind<EnemyPool>().To<EnemyPool>().AsTransient();
            Container.Bind<IEnemyService>().To<EnemyService>().AsSingle();
        }
        private void BindBullet()
        {
            Container.Bind<BulletItem>().FromInstance(_bulletPrefab).AsSingle();
            Container.Bind<BulletPool>().To<BulletPool>().AsTransient();
        }

        private void BindCar()
        {
            Container.Bind<ICarController>().To<CarController>().AsSingle();
            Container.Bind<CarModel>().To<CarModel>().AsSingle();
        }

        private void BindTurret()
        {
            Container.Bind<ITurretController>().To<TurretController>().AsSingle();
            Container.Bind<IAttackService>().To<AttackService>().AsSingle();
        }
    }
}