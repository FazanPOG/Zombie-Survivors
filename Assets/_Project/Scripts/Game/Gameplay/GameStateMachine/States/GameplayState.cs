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
        private readonly LevelScore _levelScore;
        private readonly IZombieSpawnerService _zombieSpawnerService;
        private readonly IBoostSpawnerService _boostSpawnerService;
        private readonly Environment _environment;

        private List<IDisposable> _disposables = new List<IDisposable>();
        
        public GameplayState(
            IGameStateMachine gameStateMachine,
            Player player,
            PlayerHealth playerHealth, 
            LevelScore levelScore, 
            IZombieSpawnerService zombieSpawnerService,
            IBoostSpawnerService boostSpawnerService,
            Environment environment)
        {
            _gameStateMachine = gameStateMachine;
            _player = player;
            _playerHealth = playerHealth;
            _levelScore = levelScore;
            _zombieSpawnerService = zombieSpawnerService;
            _boostSpawnerService = boostSpawnerService;
            _environment = environment;
        }
        
        public void Enter()
        {
            _disposables.Add(_playerHealth.Health.Subscribe(HandlePlayerHealthChanged));
            _disposables.Add(_levelScore.Score.Subscribe(HandleLevelProgressChanged));
            
            _zombieSpawnerService.StartSpawning(_environment.ZombieSpawnDelayMin, _environment.ZombieSpawnDelayMax, _player, _environment.MaxZombies);
            _boostSpawnerService.StartSpawning(_environment.BoostSpawnDelayMin, _environment.BoostSpawnDelayMax, _environment.MaxBoosts);
        }

        private void HandlePlayerHealthChanged(int hp)
        {
            if(hp == 0)
                _gameStateMachine.EnterIn<LoseState>();
        }
        
        private void HandleLevelProgressChanged(int progress)
        {
            if(progress == _levelScore.MaxProgress)
                _gameStateMachine.EnterIn<WinState>();
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