using System;
using System.Collections.Generic;
using _Project.Data;
using R3;

namespace _Project.Gameplay
{
    public class GameStateMachine : IGameStateMachine, IGameStateProvider
    {
        private readonly ReactiveProperty<IGameState> _currentState = new ReactiveProperty<IGameState>();
        private readonly Dictionary<Type, IGameState> _gameStates;

        public ReadOnlyReactiveProperty<IGameState> GameState => _currentState;

        public GameStateMachine(
            Player player, 
            PlayerHealth playerHealth, 
            IZombieSpawnerService spawnerService,
            IBoostSpawnerService boostSpawnerService,
            IPauseService pauseService,
            EnvironmentConfig environmentConfig)
        {
            _gameStates = new Dictionary<Type, IGameState>()
            {
                [typeof(BootState)] = new BootState(pauseService),
                [typeof(GameplayState)] = new GameplayState(this, player, playerHealth, spawnerService, boostSpawnerService, environmentConfig),
                [typeof(WinState)] = new WinState(),
                [typeof(LoseState)] = new LoseState(),
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