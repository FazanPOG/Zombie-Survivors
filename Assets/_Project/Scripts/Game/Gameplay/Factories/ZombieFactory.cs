using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    public class ZombieFactory
    {
        private readonly Zombie _zombiePrefab;
        private readonly ZombieConfig[] _zombieConfigs;
        private readonly IScoreService _scoreService;
        private readonly IPauseService _pauseService;
        private readonly IZombieCounterService _zombieCounterService;

        private readonly List<Zombie> _zombies = new List<Zombie>();
        
        public ZombieFactory(
            Zombie zombiePrefab, 
            ZombieConfig[] zombieConfigs, 
            IScoreService scoreService,
            IPauseService pauseService,
            IZombieCounterService zombieCounterService)
        {
            _zombiePrefab = zombiePrefab;
            _zombieConfigs = zombieConfigs;
            _scoreService = scoreService;
            _pauseService = pauseService;
            _zombieCounterService = zombieCounterService;
        }

        public Zombie Create(ZombieType zombieType, Vector3 spawnPosition)
        {
            var zombieConfig = GetZombieConfig(zombieType);
            
            var instance = Object.Instantiate(_zombiePrefab, spawnPosition, quaternion.identity);
            instance.Init(zombieConfig, _scoreService, _pauseService.Unregister, _zombieCounterService);
            
            _zombies.Add(instance);
            _pauseService.Register(instance);
            _zombieCounterService.Add();
            
            return instance;
        }

        public void KillAll()
        {
            foreach (var zombie in _zombies)
            {
                if (zombie != null)
                {
                    _pauseService.Unregister(zombie);
                    zombie.Kill();
                }
            }
        }
        
        private ZombieConfig GetZombieConfig(ZombieType zombieType)
        {
            ZombieConfig[] configs = _zombieConfigs.Where(x => x.ZombieType == zombieType).ToArray();
            
            if(configs.IsEmpty())
                throw new Exception();

            int randomIndex = Random.Range(0, configs.Length - 1);
            return configs[randomIndex];
        }
    }
}