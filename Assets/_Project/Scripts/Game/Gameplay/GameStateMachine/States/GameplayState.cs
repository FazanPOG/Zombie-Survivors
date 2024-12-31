using System;
using System.Collections.Generic;
using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class GameplayState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly Player _player;
        private readonly PlayerHealth _playerHealth;
        private readonly IZombieSpawnerService _zombieSpawnerService;
        private readonly IBoostSpawnerService _boostSpawnerService;
        private readonly EnvironmentConfig _environmentConfig;

        private List<IDisposable> _disposables = new List<IDisposable>();
        
        public GameplayState(
            IGameStateMachine gameStateMachine,
            Player player,
            PlayerHealth playerHealth, 
            IZombieSpawnerService zombieSpawnerService,
            IBoostSpawnerService boostSpawnerService,
            EnvironmentConfig environmentConfig)
        {
            _gameStateMachine = gameStateMachine;
            _player = player;
            _playerHealth = playerHealth;
            _zombieSpawnerService = zombieSpawnerService;
            _boostSpawnerService = boostSpawnerService;
            _environmentConfig = environmentConfig;
        }
        
        public void Enter()
        {
            _disposables.Add(_playerHealth.Health.Subscribe(HandlePlayerHealthChanged));
            
            _zombieSpawnerService.StartSpawning(_environmentConfig.ZombieSpawnDelayMin, _environmentConfig.ZombieSpawnDelayMax, _player, _environmentConfig.MaxZombies);
            _boostSpawnerService.StartSpawning(_environmentConfig.BoostSpawnDelayMin, _environmentConfig.BoostSpawnDelayMax, _environmentConfig.MaxBoosts);
        }

        private void HandlePlayerHealthChanged(int hp)
        {
            if(hp == 0)
                _gameStateMachine.EnterIn<LoseState>();
        }
        
        public void Exit()
        {
            _zombieSpawnerService.StopSpawning();
            _boostSpawnerService.StopSpawning();
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}