using UnityEngine;
using UnityEngine.Pool;

namespace Factura.Bullet
{
    public class BulletPool
    {
        private readonly ObjectPool<BulletItem> _pool;
        private readonly BulletItem _bulletPrefab;
        private readonly Transform _containerTransform;

        public BulletPool(BulletItem bulletPrefab)
        {
            _bulletPrefab = bulletPrefab;

            GameObject containerObject = new GameObject("PoolBullets");
            _containerTransform = containerObject.transform;

            _pool = new ObjectPool<BulletItem>(
                CreateBullet,
                ActivateBullet,
                DeactivateBullet,
                DestroyBullet,
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        public BulletItem GetBullet() => _pool.Get();

        private BulletItem CreateBullet()
        {
            var bullet = Object.Instantiate(_bulletPrefab, _containerTransform);
            bullet.Initialize(ReturnToPool);
            return bullet;
        }

        private void ActivateBullet(BulletItem bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void DeactivateBullet(BulletItem bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void DestroyBullet(BulletItem bullet)
        {
            Object.Destroy(bullet.gameObject);
        }

        private void ReturnToPool(BulletItem bullet)
        {
            _pool.Release(bullet);
        }
    }
}