using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick _joystick;
        
        public void Bind(DiContainer diContainer)
        {
            diContainer.Bind<FloatingJoystick>().FromInstance(_joystick).AsSingle().NonLazy();
        }
    }
}