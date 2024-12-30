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

        public LoseScreenViewPresenter(LoseScreenView view, IGameStateProvider gameStateProvider, ISceneLoaderService sceneLoaderService)
        {
            _view = view;
            _gameStateProvider = gameStateProvider;
            _sceneLoaderService = sceneLoaderService;

            _gameStateProvider.GameState.Subscribe(HandleGameState);
            _view.OnHomeButtonClicked += OnHomeButtonClicked;
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