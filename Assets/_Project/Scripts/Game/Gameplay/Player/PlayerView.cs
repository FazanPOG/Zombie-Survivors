using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const float ROTATION_SPEED = 7f;
        private const string MOVE_SPEED_KEY = "MoveSpeed";
        private const string IS_MELEE_KEY = "IsMelee";
        private const string IS_PISTOL_KEY = "IsPistol";
        private const string IS_RIFLE_KEY = "IsRifle";
        private const string DIED_KEY = "Died";
        
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private Zombie _lookTarget;
        private List<IDisposable> _disposables = new List<IDisposable>();

        public bool IsLookingToTarget { get; private set; }
        
        private void Awake() => _animator = GetComponent<Animator>();

        public void Init(PlayerMovement playerMovement, ReadOnlyReactiveProperty<int> health, ReadOnlyReactiveProperty<Zombie> lookTarget)
        {
            _playerMovement = playerMovement;
            
            //TODO
            _animator.SetBool(IS_PISTOL_KEY, true);

            _disposables.Add(health.Subscribe(value =>
            {
                if (value <= 0)
                {
                    _animator.SetTrigger(DIED_KEY);
                }
            }));

            _lookTarget = lookTarget.CurrentValue;
            _disposables.Add(lookTarget.Subscribe(newTarget =>
            {
                _lookTarget = newTarget;
            }));
        }

        private void Update()
        {
            _animator.SetFloat(MOVE_SPEED_KEY, _playerMovement.Speed);

            if (_lookTarget != null)
            {
                IsLookingToTarget = LookAt(_lookTarget.transform.position - transform.position, ROTATION_SPEED);
                return;
            }
            
            if (_playerMovement.IsMoving.CurrentValue)
                LookAt(_playerMovement.MoveDirection, ROTATION_SPEED);
            
            IsLookingToTarget = false;
        }

        private bool LookAt(Vector3 direction, float rotationSpeed, float angleOffset = 15f)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
    
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );

            return angleDifference <= angleOffset;
        }
        
        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}