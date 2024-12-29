using System;
using System.Collections.Generic;
using _Project.Data;
using R3;

namespace _Project.Gameplay
{
    public class GameplayState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly Player _player;
        private readonly PlayerHealth _playerHealth;
        private readonly LevelProgress _levelProgress;
        private readonly IZombieSpawnerService _spawnerService;

        private List<IDisposable> _disposables = new List<IDisposable>();
        
        public GameplayState(
            IGameStateMachine gameStateMachine,
            Player player,
            PlayerHealth playerHealth, 
            LevelProgress levelProgress, 
            IZombieSpawnerService spawnerService)
        {
            _gameStateMachine = gameStateMachine;
            _player = player;
            _playerHealth = playerHealth;
            _levelProgress = levelProgress;
            _spawnerService = spawnerService;
        }
        
        public void Enter()
        {
            _disposables.Add(_playerHealth.Health.Subscribe(HandlePlayerHealthChanged));
            _disposables.Add(_levelProgress.Progress.Subscribe(HandleLevelProgressChanged));
            
            _spawnerService.StartSpawning(0.2f, 1f, _player, 50);
        }

        private void HandlePlayerHealthChanged(int hp)
        {
            if(hp == 0)
                _gameStateMachine.EnterIn<LoseState>();
        }
        
        private void HandleLevelProgressChanged(int progress)
        {
            if(progress == _levelProgress.MaxProgress)
                _gameStateMachine.EnterIn<WinState>();
        }
        
        public void Exit()
        {
            _spawnerService.StopSpawning();
            
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}