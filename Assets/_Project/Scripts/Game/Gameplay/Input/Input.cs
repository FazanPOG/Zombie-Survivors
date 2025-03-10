﻿using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Gameplay
{
    public class Input : IInput, ITickable, IDisposable
    {
        private FloatingJoystick _joystick;
        private PlayerInputActions _playerInputActions;
        private bool _isJoystickEnabled;
        private IDisposable _disposable;

        public Vector2 MovementInput { get; private set; }

        public event Action OnAnyKey;

        public Input()
        {
            _playerInputActions = new PlayerInputActions();
            
            _playerInputActions.Enable();
            
            _playerInputActions.Movement.WASD.performed += MovementPerformed;
            _playerInputActions.Movement.Arrows.performed += MovementPerformed;
            
            _playerInputActions.Movement.WASD.canceled += MovementPerformed;
            _playerInputActions.Movement.Arrows.canceled += MovementPerformed;

            _playerInputActions.Movement.AnyKey.started += AnyKeyOnStarted;
        }

        public void AttachJoystick(FloatingJoystick joystick)
        {
            _joystick = joystick;
            
            _disposable = _joystick.OnDrag.Subscribe(isDragging =>
            {
                _isJoystickEnabled = isDragging;

                if (_isJoystickEnabled == false)
                    MovementInput = Vector2.zero;
            });
        }

        private void AnyKeyOnStarted(InputAction.CallbackContext _)
        {
            OnAnyKey?.Invoke();
        }

        public void Tick()
        {
            if (_isJoystickEnabled)
                MovementInput = _joystick.Direction;
        }

        private void MovementPerformed(InputAction.CallbackContext callbackContext)
        {
            if(_isJoystickEnabled == false)
                MovementInput = callbackContext.ReadValue<Vector2>();
        }

        public void Dispose()
        {
            _playerInputActions.Movement.WASD.performed -= MovementPerformed;
            _playerInputActions.Movement.Arrows.performed -= MovementPerformed;
            _playerInputActions.Movement.WASD.canceled -= MovementPerformed;
            _playerInputActions.Movement.Arrows.canceled -= MovementPerformed;
            _playerInputActions.Movement.AnyKey.started -= AnyKeyOnStarted;
            
            _disposable?.Dispose();
            _playerInputActions.Disable();
            _playerInputActions?.Dispose();
        }
    }
}