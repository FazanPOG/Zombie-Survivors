using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Data;
using _Project.Utility;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    public class ZombieSpawnerService : IZombieSpawnerService
    {
        private readonly MonoBehaviourContext _context;
        private readonly LevelProgress _levelProgress;
        private readonly ZombieFactory _zombieFactory;
        private readonly Transform[] _zombieSpawnPoints;
        private readonly Transform _playerTransform;

        private Coroutine _coroutine;
        
        public ZombieSpawnerService(
            MonoBehaviourContext context, 
            LevelProgress levelProgress, 
            ZombieFactory zombieFactory,
            Transform[] zombieSpawnPoints,
            Transform playerTransform)
        {
            _context = context;
            _levelProgress = levelProgress;
            _zombieFactory = zombieFactory;
            _zombieSpawnPoints = zombieSpawnPoints;
            _playerTransform = playerTransform;
        }
        
        public void StartSpawning(float minSpawnDelay, float maxSpawnDelay, Player zombieTarget, int maxZombies)
        {
            _coroutine = _context.StartCoroutine(Spawning(minSpawnDelay, maxSpawnDelay, zombieTarget, maxZombies));
        }

        public void StopSpawning()
        {
            if (_coroutine == null)
                throw new NullReferenceException("Trying to stop spawning, but is wont start");
            
            _context.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator Spawning(float minSpawnDelay, float maxSpawnDelay, Player zombieTarget, int maxZombies)
        {
            int currentSpawned = 0;
            while (currentSpawned < maxZombies && zombieTarget != null)
            {
                int levelProgress = _levelProgress.Progress.CurrentValue;

                Zombie zombieInstance;
                List<Transform> zombieDistantSpawnPoints = GetDistantSpawnPoints(_playerTransform.position, _zombieSpawnPoints);
                Vector3 zombieSpawnPosition = GetRandomSpawnPosition(zombieDistantSpawnPoints);
                
                if(levelProgress <= 33)
                    zombieInstance = _zombieFactory.Create(ZombieType.Easy, zombieSpawnPosition);
                else if(levelProgress <= 66)
                    zombieInstance = _zombieFactory.Create(ZombieType.Medium, zombieSpawnPosition);
                else
                    zombieInstance = _zombieFactory.Create(ZombieType.Hard, zombieSpawnPosition);

                currentSpawned++;
                zombieInstance.SetTarget(zombieTarget);
                float randomSpawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
                
                yield return new WaitForSeconds(randomSpawnDelay);
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
            int randomIndex = Random.Range(0, points.Count - 1);
            return points[randomIndex].position;
        }
    }
}