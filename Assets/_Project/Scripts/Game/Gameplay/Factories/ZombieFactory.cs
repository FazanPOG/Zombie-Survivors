using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    public class ZombieFactory
    {
        private readonly Zombie _zombiePrefab;
        private readonly Transform[] _zombieSpawnPoints;
        private readonly Player _player;

        public ZombieFactory(Zombie zombiePrefab, Transform[] zombieSpawnPoints, Player player)
        {
            _zombiePrefab = zombiePrefab;
            _zombieSpawnPoints = zombieSpawnPoints;
            _player = player;
        }

        public Zombie Create()
        {
            Transform randomSpawnPoint = GetRandomZombieSpawnPoint();
            var instance = Object.Instantiate(_zombiePrefab, randomSpawnPoint.transform.position, quaternion.identity);
            instance.Init(_player);
            return instance;
        }

        private Transform GetRandomZombieSpawnPoint()
        {
            int randomIndex = Random.Range(0, _zombieSpawnPoints.Length - 1);
            return _zombieSpawnPoints[randomIndex].transform;
        }
    }
}