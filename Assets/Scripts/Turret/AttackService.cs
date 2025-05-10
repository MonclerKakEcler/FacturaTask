using UnityEngine;
using Factura.Bullet;

namespace Factura.Turret
{
    public interface IAttackService
    {
        void ProccesAttack(Vector3 position, Quaternion angle, Vector3 direction);
    }

    public class AttackService : IAttackService
    {
        private const float kForceMagnitude = 100;
        private readonly BulletPool _bulletPool;

        public AttackService(BulletPool bulletPool)
        {
            _bulletPool = bulletPool;
        }

        public void ProccesAttack(Vector3 position, Quaternion angle, Vector3 direction)
        {
            var bullet = _bulletPool.GetBullet();
            bullet.transform.position = position;
            bullet.transform.rotation = angle;

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(direction * kForceMagnitude, ForceMode.Impulse);

            bullet.Shoot();
        }
    }
}
