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
    public class BoostSpawningService : IBoostSpawnerService
    {
        private readonly MonoBehaviourContext _context;
        private readonly Transform[] _boostSpawnPoint;
        private readonly Transform _playerTransform;
        private readonly BoostFactory _boostFactory;
        private readonly IBoostCounterService _boostCounterService;
        private readonly LevelScore _levelScore;

        private Coroutine _coroutine;
        
        public BoostSpawningService(
            MonoBehaviourContext context, 
            Transform[] boostSpawnPoint,
            Transform playerTransform,
            BoostFactory boostFactory,
            IBoostCounterService boostCounterService,
            LevelScore levelScore)
        {
            _context = context;
            _boostSpawnPoint = boostSpawnPoint;
            _playerTransform = playerTransform;
            _boostFactory = boostFactory;
            _boostCounterService = boostCounterService;
            _levelScore = levelScore;
        }
        
        public void StartSpawning(float minSpawnDelay, float maxSpawnDelay, int maxBoosts)
        {
            _coroutine = _context.StartCoroutine(Spawning(minSpawnDelay, maxSpawnDelay, maxBoosts));
        }

        public void StopSpawning()
        {
            if (_coroutine == null)
                throw new NullReferenceException("Trying to stop spawning, but is wont start");
            
            _context.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator Spawning(float minSpawnDelay, float maxSpawnDelay, int maxBoosts)
        {
            while (_playerTransform != null)
            {
                yield return new WaitUntil(() => _boostCounterService.Count.CurrentValue < maxBoosts);

                int score = _levelScore.Score.CurrentValue;
                List<Transform> distantSpawnPoints = GetDistantSpawnPoints(_playerTransform.position, _boostSpawnPoint);
                Vector3 spawnPosition = GetRandomSpawnPosition(distantSpawnPoints);
                
                if (_boostFactory.CanCreateWeaponBoost(score))
                    _boostFactory.CreateNextWeaponBoost(spawnPosition);
                else
                    _boostFactory.CreateRandomNonWeaponBoost(spawnPosition);

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