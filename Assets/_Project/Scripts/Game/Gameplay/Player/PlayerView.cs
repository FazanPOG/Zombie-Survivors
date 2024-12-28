using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const float ROTATION_SPEED = 10f;
        private const string MOVE_SPEED_KEY = "MoveSpeed";
        private const string IS_MELEE_KEY = "IsMelee";
        private const string IS_PISTOL_KEY = "IsPistol";
        private const string IS_RIFLE_KEY = "IsRifle";
        private const string DIED_KEY = "Died";
        
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private Zombie _lookTarget;
        private List<IDisposable> _disposables = new List<IDisposable>();

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
                
                if (_lookTarget == null)
                    transform.rotation = Quaternion.identity;
            }));
        }

        private void Update()
        {
            _animator.SetFloat(MOVE_SPEED_KEY, _playerMovement.Speed);

            if (_lookTarget != null)
                LookAt(_lookTarget.transform.position - transform.position);
            else if(_playerMovement.IsMoving.CurrentValue)
                LookAt(_playerMovement.MoveDirection);
        }

        private void LookAt(Vector3 direction)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                targetRotation, 
                Time.deltaTime * ROTATION_SPEED
            );
        }
        
        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}