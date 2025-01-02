using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Data;
using _Project.Utility;
using ModestTree;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    public class ZombieSpawnerService : IZombieSpawnerService
    {
        private readonly MonoBehaviourContext _context;
        private readonly LevelScore _levelScore;
        private readonly ZombieFactory _zombieFactory;
        private readonly Transform[] _zombieSpawnPoints;
        private readonly Transform _playerTransform;
        private readonly IZombieCounterService _zombieCounterService;

        private Coroutine _coroutine;
        private int _maxZombies;
        
        public ZombieSpawnerService(
            MonoBehaviourContext context, 
            LevelScore levelScore, 
            ZombieFactory zombieFactory,
            Transform[] zombieSpawnPoints,
            Transform playerTransform,
            IZombieCounterService zombieCounterService)
        {
            _context = context;
            _levelScore = levelScore;
            _zombieFactory = zombieFactory;
            _zombieSpawnPoints = zombieSpawnPoints;
            _playerTransform = playerTransform;
            _zombieCounterService = zombieCounterService;

            _levelScore.Score.Skip(1).Subscribe(UpdateMaxZombies);
        }

        private void UpdateMaxZombies(int score)
        {
            if(_maxZombies == 0)
                return;

            _maxZombies += (score / 2);
        }
        
        public void StartSpawning(float minSpawnDelay, float maxSpawnDelay, Player zombieTarget, int maxZombies)
        {
            _maxZombies = maxZombies;
            _coroutine = _context.StartCoroutine(Spawning(minSpawnDelay, maxSpawnDelay, zombieTarget));
        }

        public void StopSpawning()
        {
            if (_coroutine == null)
                throw new NullReferenceException("Trying to stop spawning, but is wont start");
            
            _context.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator Spawning(float minSpawnDelay, float maxSpawnDelay, Player zombieTarget)
        {
            while (zombieTarget != null)
            {
                yield return new WaitUntil(() => _zombieCounterService.Count.CurrentValue < _maxZombies);
                
                int score = _levelScore.Score.CurrentValue;
                
                List<Transform> zombieDistantSpawnPoints = GetDistantSpawnPoints(_playerTransform.position, _zombieSpawnPoints);
                Vector3 zombieSpawnPosition = GetRandomSpawnPosition(zombieDistantSpawnPoints);

                var zombieInstance = CreateRandomZombie(zombieSpawnPosition);
                if (score > 0 && score % 50 == 0)
                    zombieInstance = _zombieFactory.Create(ZombieType.Boss, zombieSpawnPosition);
                
                zombieInstance.SetTarget(zombieTarget);
                float randomSpawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
                
                yield return new WaitForSeconds(randomSpawnDelay);
            }
        }

        private Zombie CreateRandomZombie(Vector3 zombieSpawnPosition)
        {
            int number = Random.Range(0, 3);
            switch (number)
            {
                case 0:
                    return _zombieFactory.Create(ZombieType.Easy, zombieSpawnPosition);
                case 1:
                    return _zombieFactory.Create(ZombieType.Medium, zombieSpawnPosition);
                case 2:
                    return _zombieFactory.Create(ZombieType.Hard, zombieSpawnPosition);
                
                default:
                    throw new Exception();
            }
        }
        
        private List<Transform> GetDistantSpawnPoints(Vector3 playerPosition, Transform[] transforms, float minDistance = 20f)
        {
            List<Transform> distantTransforms = new List<Transform>();

            foreach (var item in transforms)
            {
                float distance = Vector3.Distance(playerPosition, item.position);
                
                if(distance >= minDistance)
                    distantTransforms.Add(item);
            }

            if(distantTransforms.IsEmpty())
                throw new Exception();
            
            return distantTransforms;
        }

        private Vector3 GetRandomSpawnPosition(List<Transform> points)
        {
            int randomIndex = Random.Range(0, points.Count);
            return points[randomIndex].position;
        }
    }
}