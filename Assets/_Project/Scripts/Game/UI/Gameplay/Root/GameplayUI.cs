using System;
using System.Collections.Generic;
using _Project.Data;
using _Project.Gameplay;
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

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private DiContainer _container;
        private ClickToStartView _clickToStartViewInstance;
        private HealthBarView _healthBarViewInstance;
        
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

            var clickToStartScreenViewPresenter = new ClickToStartScreenViewPresenter(_clickToStartViewInstance, input, gameStateMachine);
            _disposables.Add(clickToStartScreenViewPresenter);
            
            var healthBarPresenter = new HealthBarViewPresenter(_healthBarViewInstance, playerHealth);
            _disposables.Add(healthBarPresenter);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}