﻿using System;
using _Project.Gameplay;

namespace _Project.UI
{
    public class ClickToStartScreenViewPresenter : IDisposable
    {
        private readonly ClickToStartView _view;
        private readonly IInput _input;
        private readonly IGameStateMachine _gameStateMachine;

        public ClickToStartScreenViewPresenter(ClickToStartView view, IInput input, IGameStateMachine gameStateMachine)
        {
            _view = view;
            _input = input;
            _gameStateMachine = gameStateMachine;

            _view.Show();
            _input.OnAnyKey += OnAnyKeyClicked;
        }

        private void OnAnyKeyClicked()
        {
            _view.Hide();
            _gameStateMachine.EnterIn<GameplayState>();
        }

        public void Dispose()
        {
            _input.OnAnyKey -= OnAnyKeyClicked;
        }
    }
}