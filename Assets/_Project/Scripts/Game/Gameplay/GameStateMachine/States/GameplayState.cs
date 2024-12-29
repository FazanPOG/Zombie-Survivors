using System;
using System.Collections.Generic;
using _Project.Data;
using R3;

namespace _Project.Gameplay
{
    public class GameplayState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly PlayerHealth _playerHealth;
        private readonly LevelProgress _levelProgress;

        private List<IDisposable> _disposables = new List<IDisposable>();
        
        public GameplayState(IGameStateMachine gameStateMachine, PlayerHealth playerHealth, LevelProgress levelProgress)
        {
            _gameStateMachine = gameStateMachine;
            _playerHealth = playerHealth;
            _levelProgress = levelProgress;
        }
        
        public void Enter()
        {
            _disposables.Add(_playerHealth.Health.Subscribe(HandlePlayerHealthChanged));
            _disposables.Add(_levelProgress.Progress.Subscribe(HandleLevelProgressChanged));
            
            //Start spawning zombie
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
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}