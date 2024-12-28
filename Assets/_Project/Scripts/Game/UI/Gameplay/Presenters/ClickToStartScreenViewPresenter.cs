using _Project.Gameplay;

namespace _Project.UI
{
    public class ClickToStartScreenViewPresenter
    {
        private readonly ClickToStartView _view;
        private readonly IGameStateMachine _gameStateMachine;

        public ClickToStartScreenViewPresenter(ClickToStartView view, IInput input, IGameStateMachine gameStateMachine)
        {
            _view = view;
            _gameStateMachine = gameStateMachine;

            _view.Show();
            input.OnAnyKey += OnAnyKeyClicked;
        }

        private void OnAnyKeyClicked()
        {
            _view.Hide();
            _gameStateMachine.EnterIn<GameplayState>();
        }
    }
}