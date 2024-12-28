using System;
using System.Collections.Generic;
using R3;

namespace _Project.Gameplay
{
    public class GameStateMachine : IGameStateMachine, IGameStateProvider
    {
        private readonly ReactiveProperty<IGameState> _currentState = new ReactiveProperty<IGameState>();
        private readonly Dictionary<Type, IGameState> _gameStates;

        public ReadOnlyReactiveProperty<IGameState> GameState => _currentState;

        public GameStateMachine()
        {
            _gameStates = new Dictionary<Type, IGameState>()
            {
                [typeof(BootState)] = new BootState(),
                [typeof(GameplayState)] = new GameplayState(),
            };
        }
        
        public void EnterIn<T>() where T : IGameState
        {
            if(_gameStates.TryGetValue(typeof(T), out IGameState gameState) == false)
                throw new MissingMemberException($"Game state does not exist: {typeof(T)}");
            
            _currentState.Value?.Exit();
            _currentState.Value = gameState;
            _currentState.Value.Enter();
        }
    }
}