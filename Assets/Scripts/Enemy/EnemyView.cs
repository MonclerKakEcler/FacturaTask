using UnityEngine;
using System;
using Factura.Car;
using Factura.Health;
using Factura.Bullet;

namespace Factura.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Transform _enemyTransform; 
        [SerializeField] private Animator _animator;
        [SerializeField] private BaseHealth _health;

        public Transform EnemyTansform => _enemyTransform;
        public Animator Animator => _animator;
        public BaseHealth Health => _health;

        public event Action OnBulletHit;
        public event Action OnCarHit;
        public event Action RunToCar;

        public void MoveTowards(Vector3 target, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        public void LookAtSmooth(Vector3 target, float rotationSpeed)
        {
            Vector3 direction = (target - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<BulletItem>(out var bullet))
            {
                bullet.ReturnToPool();
                OnBulletHit?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICarView>(out var car))
            {
                OnCarHit?.Invoke();
                car.InvokeHitByEnemy();
            }
        }

        private void Update()
        {
            RunToCar?.Invoke();
        }
    }
}