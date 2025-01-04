using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.Gameplay;
using _Project.Root;
using _Project.Utility;
using R3;

namespace _Project.UI
{
    public class LoseScreenViewPresenter
    {
        private const int CURRENCY_CONTINUE_VALUE = 100;
        private const string REWARDED_KEY = "CONTINUE GAME";
        
        private readonly LoseScreenView _view;
        private readonly IGameStateProvider _gameStateProvider;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly Player _player;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameDataProvider _gameDataProvider;
        private readonly LevelScore _levelScore;
        private readonly IADService _adService;
        private readonly AudioPlayer _audioPlayer;

        public LoseScreenViewPresenter(
            LoseScreenView view, 
            IGameStateProvider gameStateProvider, 
            ISceneLoaderService sceneLoaderService,
            Player player,
            IGameStateMachine gameStateMachine,
            ILocalizationProvider localizationProvider,
            IGameDataProvider gameDataProvider,
            LevelScore levelScore,
            IADService adService,
            AudioPlayer audioPlayer)
        {
            _view = view;
            _gameStateProvider = gameStateProvider;
            _sceneLoaderService = sceneLoaderService;
            _player = player;
            _gameStateMachine = gameStateMachine;
            _gameDataProvider = gameDataProvider;
            _levelScore = levelScore;
            _adService = adService;
            _audioPlayer = audioPlayer;

            _view.SetTitleText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.LOSE_TITLE_KEY));
            _view.SetContinueText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.CONTINUE_KEY) + "?");
            _view.SetADButtonText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.CONTINUE_KEY));
            
            _gameStateProvider.GameState.Subscribe(HandleGameState);
            _view.OnHomeButtonClicked += OnHomeButtonClicked;
            _view.OnADContinueButtonClicked += OnADContinueButtonClicked;
            _view.OnCurrencyContinueButtonClicked += OnCurrencyContinueButtonClicked;
            _adService.OnRewardedReward += OnRewardedReward;
        }

        private void OnRewardedReward(string key)
        {
            if(key == REWARDED_KEY)
                ContinueGame();
        }

        private void OnCurrencyContinueButtonClicked()
        {
            if(_gameDataProvider.GameDataProxy.HardCurrency.Value < CURRENCY_CONTINUE_VALUE)
                return;

            _audioPlayer.PlayButtonClickSound();
            _gameDataProvider.GameDataProxy.HardCurrency.Value -= CURRENCY_CONTINUE_VALUE;
            ContinueGame();
        }

        private void OnADContinueButtonClicked()
        {
            _audioPlayer.PlayButtonClickSound();
            
            if(_adService.IsRewardedAvailable)
                _adService.ShowRewarded(REWARDED_KEY);
        }

        private void ContinueGame()
        {
            _player.Revive();
            _view.Hide();
            _gameStateMachine.EnterIn<GameplayState>();
        }
        
        private void HandleGameState(IGameState gameState)
        {
            if (gameState is LoseState)
            {
                _view.SetCurrencyValueText($"+{_levelScore.Score.CurrentValue}");
                _view.Show();
                _view.SetCurrencyContinueButtonActiveState(_gameDataProvider.GameDataProxy.HardCurrency.CurrentValue >= CURRENCY_CONTINUE_VALUE);
            }
            
        }
        
        private void OnHomeButtonClicked()
        {
            _audioPlayer.PlayButtonClickSound();
            _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu);
        }
    }
}