using System;
using System.Collections.Generic;
using _Project.Data;
using _Project.Gameplay;
using _Project.Root;
using UnityEngine;
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
        [SerializeField] private LevelProgressView _levelProgressViewPrefab;
        [SerializeField] private PopupButtonView _pauseButtonView;
        [Header("Popups")]
        [SerializeField] private PausePopupView _pausePopupView;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private DiContainer _container;
        private ClickToStartView _clickToStartViewInstance;
        private HealthBarView _healthBarViewInstance;
        private LevelProgressView _levelProgressViewInstance;
        
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
            _levelProgressViewInstance = _container.InstantiatePrefabForComponent<LevelProgressView>(_levelProgressViewPrefab, _HUDParent);
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
            var playerHealth = _container.Resolve<PlayerHealth>();
            var levelProgress = _container.Resolve<LevelProgress>();
            var pauseService = _container.Resolve<IPauseService>();
            var sceneLoader = _container.Resolve<ISceneLoaderService>();

            var clickToStartScreenViewPresenter = new ClickToStartScreenViewPresenter(_clickToStartViewInstance, input, gameStateMachine);
            _disposables.Add(clickToStartScreenViewPresenter);
            
            var healthBarPresenter = new HealthBarViewPresenter(_healthBarViewInstance, playerHealth);
            _disposables.Add(healthBarPresenter);
            
            var levelProgressViewPresenter = new LevelProgressViewPresenter(levelProgress, _levelProgressViewInstance);
            _disposables.Add(levelProgressViewPresenter);

            var pausePopupViewPresenter = new PausePopupViewPresenter(_pausePopupView, _pauseButtonView, pauseService, sceneLoader);
            _disposables.Add(pausePopupViewPresenter);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}