using System.Collections.Generic;
using _Project.Audio;
using UnityEngine;
using Zenject;

namespace _Project.Gameplay
{
    public class BoostFactory
    {
        private const int WEAPON_BOOST_SPAWN_KOEFF = 5;
        
        private readonly DiContainer _diContainer;
        private readonly BaseBoost[] _boostPrefabs;
        private readonly WeaponBoost _weaponBoostPrefab;
        private readonly AudioPlayer _audioPlayer;
        private readonly IBoostCounterService _boostCounterService;
        private readonly Queue<WeaponConfig> _weaponConfigsQueue = new Queue<WeaponConfig>();

        private int _weaponCreatedCounter = 1;
        
        public BoostFactory(
            DiContainer diContainer,
            BaseBoost[] boostPrefabs,
            WeaponBoost weaponBoostPrefab,
            WeaponConfig[] weaponBoostConfigs,
            AudioPlayer audioPlayer,
            IBoostCounterService boostCounterService)
        {
            _diContainer = diContainer;
            _boostPrefabs = boostPrefabs;
            _weaponBoostPrefab = weaponBoostPrefab;
            _audioPlayer = audioPlayer;
            _boostCounterService = boostCounterService;
            
            foreach (var weaponConfig in weaponBoostConfigs)
                _weaponConfigsQueue.Enqueue(weaponConfig);
        }

        public BaseBoost CreateRandomNonWeaponBoost(Vector3 spawnPosition)
        {
            var prefab = GetRandomBoostPrefab();
            
            var instance = _diContainer.InstantiatePrefabForComponent<BaseBoost>(prefab, spawnPosition, Quaternion.identity, null);
            instance.Init(_audioPlayer, _boostCounterService);
            
            _boostCounterService.Add();
            return instance;
        }

        public bool CanCreateWeaponBoost(int score)
        {
            bool canCreate = IsInRange(score, (_weaponCreatedCounter - 1) * WEAPON_BOOST_SPAWN_KOEFF,
                _weaponCreatedCounter * WEAPON_BOOST_SPAWN_KOEFF);
            
            return canCreate;
        }
        
        public WeaponBoost CreateNextWeaponBoost(Vector3 spawnPosition)
        {
            var config = GetWeaponBoostConfig();
            
            var instance = _diContainer.InstantiatePrefabForComponent<WeaponBoost>(_weaponBoostPrefab, spawnPosition, Quaternion.identity, null);
            instance.Init(config);
            instance.Init(_audioPlayer, _boostCounterService);
            
            _boostCounterService.Add();
            _weaponCreatedCounter++;
            return instance;
        }
        
        private BaseBoost GetRandomBoostPrefab()
        {
            int index = Random.Range(0, _boostPrefabs.Length);
            return _boostPrefabs[index];
        }
        
        private WeaponConfig GetWeaponBoostConfig()
        {
            return _weaponConfigsQueue.Dequeue();
        }
        
        private bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}