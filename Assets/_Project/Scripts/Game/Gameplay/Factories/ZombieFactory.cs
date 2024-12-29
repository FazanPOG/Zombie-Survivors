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
        private readonly ILevelProgressService _levelProgressService;

        private List<Zombie> _zombies = new List<Zombie>();
        
        public ZombieFactory(
            Zombie zombiePrefab, 
            ZombieConfig[] zombieConfigs, 
            ILevelProgressService levelProgressService)
        {
            _zombiePrefab = zombiePrefab;
            _zombieConfigs = zombieConfigs;
            _levelProgressService = levelProgressService;
        }

        public Zombie Create(ZombieType zombieType, Vector3 spawnPosition)
        {
            var zombieConfig = GetZombieConfig(zombieType);
            
            var instance = Object.Instantiate(_zombiePrefab, spawnPosition, quaternion.identity);
            instance.Init(zombieConfig, _levelProgressService);

            _zombies.Add(instance);
            
            return instance;
        }

        public void KillAll()
        {
            foreach (var zombie in _zombies)
            {
                if(zombie != null)
                    zombie.Kill();
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