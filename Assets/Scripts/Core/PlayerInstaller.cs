using UnityEngine;
using Zenject;
using Factura.Car;
using Factura.Turret;

namespace Factura.Core
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private TurretView _turretView;
        [SerializeField] private CarView _carView;

        public override void InstallBindings()
        {
            BindTurret();
            BindCar();
        }

        private void BindTurret()
        {
            Container.Bind<ITurretView>().FromInstance(_turretView).AsSingle();

            var controller = Container.Resolve<ITurretController>();
            controller.Init(_turretView);
        }

        private void BindCar()
        {
            Container.Bind<ICarView>().FromInstance(_carView).AsSingle();

            var controller = Container.Resolve<ICarController>();
            controller.Init(_carView);
        }
    }
}