using System.Collections.Generic;
using UnityEngine;

namespace Factura.Enemy
{
    public interface IEnemyService
    {
        void SpawnEnemies();
        void ClearEnemies();

    }

    public class EnemyService : IEnemyService
    {
        private const int kEnemyCount = 50;
        private const float kSpawnRadius = 20f;
        private const float kSpawnXMin = -20f;
        private const float kSpawnXMax = 20f;
        private const float kSpawnZStart = 0f;
        private const float kSpawnZEnd = 400f;
        private const float kSpawnZStep = 20f;

        private readonly EnemyPool _enemyPool;
        private readonly List<EnemyView> _activeEnemies = new();

        public EnemyService(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
        }

        public void SpawnEnemies()
        {
            int spawned = 0;
            float currentZ = kSpawnZStart;

            while (spawned < kEnemyCount && currentZ <= kSpawnZEnd)
            {
                float x = Random.Range(kSpawnXMin, kSpawnXMax);
                Vector3 spawnPosition = new Vector3(x, 0f, currentZ);

                if (!IsEnemyNearby(spawnPosition, kSpawnRadius))
                {
                    var enemy = _enemyPool.GetEnemy();
                    enemy.transform.position = spawnPosition;

                    _activeEnemies.Add(enemy);
                    spawned++;
                }

                currentZ += kSpawnZStep;
            }
        }

        public void ClearEnemies()
        {
            foreach (var enemy in _activeEnemies)
            {
                if (enemy != null)
                {
                    _enemyPool.ReturnToPool(enemy);
                }
            }

            _activeEnemies.Clear();
            _enemyPool.ClearPool();
        }

        private bool IsEnemyNearby(Vector3 position, float radius)
        {
            foreach (var enemy in _activeEnemies)
            {
                if (enemy == null) continue;

                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance < radius)
                    return true;
            }

            return false;
        }
    }
}
