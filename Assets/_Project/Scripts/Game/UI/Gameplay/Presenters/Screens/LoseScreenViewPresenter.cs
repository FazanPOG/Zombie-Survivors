using _Project.Gameplay;
using _Project.Root;
using _Project.Utility;
using R3;

namespace _Project.UI
{
    public class LoseScreenViewPresenter
    {
        private readonly LoseScreenView _view;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly Player _player;
        private readonly IGameStateMachine _gameStateMachine;

        public LoseScreenViewPresenter(
            LoseScreenView view, 
            IGameStateProvider gameStateProvider, 
            ISceneLoaderService sceneLoaderService,
            Player player,
            IGameStateMachine gameStateMachine)
        {
            _view = view;
            _gameStateProvider = gameStateProvider;
            _sceneLoaderService = sceneLoaderService;
            _player = player;
            _gameStateMachine = gameStateMachine;

            _gameStateProvider.GameState.Subscribe(HandleGameState);
            _view.OnHomeButtonClicked += OnHomeButtonClicked;
            _view.OnADContinueButtonClicked += OnADContinueButtonClicked;
        }

        private void OnADContinueButtonClicked()
        {
            _player.Revive();
            _view.Hide();
            _gameStateMachine.EnterIn<GameplayState>();
        }

        private void HandleGameState(IGameState gameState)
        {
            if(gameState is LoseState)
                _view.Show();
        }
        
        private void OnHomeButtonClicked()
        {
            _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu);
        }
    }
}