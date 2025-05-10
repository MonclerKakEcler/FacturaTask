using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Factura.Enemy
{
    public class EnemyPool
    {
        private readonly DiContainer _diContainer;
        private readonly EnemyView _enemyPrefab;
        private readonly Transform _containerTransform;
        private readonly ObjectPool<EnemyView> _pool;
        private readonly Dictionary<EnemyView, EnemyController> _controllers = new();

        public EnemyPool(DiContainer diContainer, EnemyView enemyPrefab)
        {
            _diContainer = diContainer;
            _enemyPrefab = enemyPrefab;

            GameObject containerObject = new GameObject("PoolEnemies");
            _containerTransform = containerObject.transform;

            _pool = new ObjectPool<EnemyView>(
                CreateEnemy,
                OnGet,
                OnRelease,
                OnDestroy,
                collectionCheck: false,
                defaultCapacity: 10
            );
        }

        public EnemyView GetEnemy() => _pool.Get();

        public void ReturnToPool(EnemyView view)
        {
            _pool.Release(view);
        }

        public void ClearPool()
        {
            foreach (var pair in _controllers)
            {
                var view = pair.Key;
                if (view != null)
                {
                    Object.Destroy(view.gameObject);
                }
            }

            _controllers.Clear();
            _pool.Clear();
        }

        private EnemyView CreateEnemy()
        {
            var view = Object.Instantiate(_enemyPrefab, _containerTransform);
            var model = new EnemyModel();
            var controller = _diContainer.Resolve<EnemyController>();

            _controllers[view] = controller;
            controller.Init(view, model, this);
            return view;
        }

        private void OnGet(EnemyView view)
        {
            view.gameObject.SetActive(true);

            if (_controllers.TryGetValue(view, out var controller))
            {
                var model = new EnemyModel();
                controller.ResetModel(model);
            }
        }

        private void OnRelease(EnemyView view)
        {
            view.gameObject.SetActive(false);
        }

        private void OnDestroy(EnemyView view)
        {
            Object.Destroy(view.gameObject);
            _controllers.Remove(view);
        }
    }
}
