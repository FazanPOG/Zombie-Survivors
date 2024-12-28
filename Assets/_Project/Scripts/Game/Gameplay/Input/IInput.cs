using System;
using UnityEngine;

namespace _Project.Gameplay
{
    public interface IInput
    {
        Vector2 MovementInput { get; }

        void AttachJoystick(FloatingJoystick joystick);
        
        event Action OnAnyKey;
    }
}