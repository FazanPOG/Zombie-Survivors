using _Project.Gameplay;
using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private ClickToStartView _clickToStartView;
        [Header("Other")]
        [SerializeField] private FloatingJoystick _joystick;

        private DiContainer _container;
        
        public void Bind(DiContainer diContainer)
        {
            _container = diContainer;

            AttachJoystick();
            BindPresenters();
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
            
            new ClickToStartScreenViewPresenter(_clickToStartView, input, gameStateMachine);
        }
    }
}