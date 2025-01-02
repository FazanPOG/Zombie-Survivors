using System;
using System.Collections.Generic;
using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.Gameplay;
using _Project.Root;
using UnityEngine;
using YG;
using Zenject;

namespace _Project.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [Header("Root")] 
        [SerializeField] private RectTransform _screensParent;
        [SerializeField] private RectTransform _popupsParent;
        [SerializeField] private RectTransform _HUDParent;
        [Header("Screens")]
        [SerializeField] private ClickToStartView _clickToStartViewPrefab;
        [Header("HUD")]
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private HealthBarView _healthBarViewPrefab;
        [SerializeField] private LevelScoreView levelScoreViewPrefab;
        [SerializeField] private PopupButtonView _pauseButtonView;
        [Header("Popups")]
        [SerializeField] private PausePopupView _pausePopupView;
        [SerializeField] private LoseScreenView _loseScreenView;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private DiContainer _container;
        private ClickToStartView _clickToStartViewInstance;
        private HealthBarView _healthBarViewInstance;
        private LevelScoreView _levelScoreViewInstance;
        
        public void Bind(DiContainer diContainer)
        {
            _container = diContainer;

            AttachJoystick();
            CreateViews();
            BindPresenters();
        }

        private void CreateViews()
        {
            _clickToStartViewInstance = _container.InstantiatePrefabForComponent<ClickToStartView>(_clickToStartViewPrefab, _screensParent);
            _healthBarViewInstance = _container.InstantiatePrefabForComponent<HealthBarView>(_healthBarViewPrefab, _HUDParent);
            _levelScoreViewInstance = _container.InstantiatePrefabForComponent<LevelScoreView>(levelScoreViewPrefab, _HUDParent);
        }

        private void AttachJoystick()
        {
            var input = _container.Resolve<IInput>();
            input.AttachJoystick(_joystick);
        }
        
        private void BindPresenters()
        {
            var input = _container.Resolve<IInput>();
            var gameStateMachine = _container.Resolve<IGameStateMachine>();
            var gameStateProvider = _container.Resolve<IGameStateProvider>();
            var playerHealth = _container.Resolve<PlayerHealth>();
            var levelProgress = _container.Resolve<LevelScore>();
            var pauseService = _container.Resolve<IPauseService>();
            var sceneLoader = _container.Resolve<ISceneLoaderService>();
            var localizationProvider = _container.Resolve<ILocalizationProvider>();
            var gameDataProvider = _container.Resolve<IGameDataProvider>();
            var player = _container.Resolve<Player>();
            var adService = _container.Resolve<IADService>();
            var audioPlayer = _container.Resolve<AudioPlayer>();

            new ClickToStartScreenViewPresenter(_clickToStartViewInstance, input, gameStateMachine, localizationProvider);
            
            var healthBarPresenter = new HealthBarViewPresenter(_healthBarViewInstance, playerHealth);
            _disposables.Add(healthBarPresenter);
            
            var levelProgressViewPresenter = new LevelScoreViewPresenter(levelProgress, _levelScoreViewInstance, localizationProvider);
            _disposables.Add(levelProgressViewPresenter);

            var pausePopupViewPresenter = new PausePopupViewPresenter(_pausePopupView, _pauseButtonView, pauseService, sceneLoader, localizationProvider);
            _disposables.Add(pausePopupViewPresenter);

            new LoseScreenViewPresenter(
                _loseScreenView, 
                gameStateProvider, 
                sceneLoader, 
                player, 
                gameStateMachine, 
                localizationProvider,
                gameDataProvider,
                levelProgress,
                adService,
                audioPlayer);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}