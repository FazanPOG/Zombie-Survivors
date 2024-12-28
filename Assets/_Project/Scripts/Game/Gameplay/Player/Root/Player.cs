using System;
using UnityEngine;

namespace _Project.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _hand;
        [SerializeField] private WeaponConfig _testWeapon; //TODO
        [SerializeField, TextArea(0, 10)] private string DEBUG_STRING; 
        
        private PlayerMovement _movement;
        
        public void Init(IInput input)
        {
            _movement = new PlayerMovement(transform, input);
            new PlayerWeaponHandler(_testWeapon, _hand);
            _view.Init(_movement);
        }

        private void Update()
        {
            UpdateDebugString();
            _movement.Update();
        }

        private void UpdateDebugString()
        {
            DEBUG_STRING = String.Empty;
            DEBUG_STRING += $"Speed: {_movement.Speed}";
        }
    }
}