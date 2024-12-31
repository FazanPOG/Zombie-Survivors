using System;
using System.Collections.Generic;
using _Project.Audio;
using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const float ROTATION_SPEED_TO_TARGET = 15f;
        private const float ROTATION_SPEED_TO_MOVE_DIR = 7f;
        private const string MOVE_SPEED_KEY = "MoveSpeed";
        private const string IS_MELEE_KEY = "IsMelee";
        private const string IS_PISTOL_KEY = "IsPistol";
        private const string IS_RIFLE_KEY = "IsRifle";
        private const string DIED_KEY = "Died";

        [SerializeField] private AudioClip _takeDamageSFX;
        [SerializeField] private AudioClip _deathSFX;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private PlayerWeaponHandler _weaponHandler;
        private Zombie _lookTarget;
        private int _previousHealth;
        private bool _died;

        public bool IsLookingToTarget { get; private set; }
        
        private void Awake() => _animator = GetComponent<Animator>();

        public void Init(
            PlayerMovement playerMovement, 
            PlayerWeaponHandler weaponHandler, 
            PlayerHealth health, 
            ReadOnlyReactiveProperty<Zombie> lookTarget,
            AudioPlayer audioPlayer)
        {
            _playerMovement = playerMovement;
            _weaponHandler = weaponHandler;
            _previousHealth = health.Health.CurrentValue;
            
            _disposables.Add(health.Health.Subscribe(value =>
            {
                if (value < _previousHealth)
                {
                    audioPlayer.PlaySoundOneShot(_takeDamageSFX);
                }

                if (value <= 0)
                {
                    _died = true;
                    audioPlayer.PlaySoundOneShot(_deathSFX);
                    _animator.SetBool(DIED_KEY, _died);
                }

                if (health.Health.CurrentValue == health.MaxHealth)
                {
                    _died = false;
                    _animator.SetBool(DIED_KEY, _died);
                }
                
                _previousHealth = value;
            }));

            _lookTarget = lookTarget.CurrentValue;
            _disposables.Add(lookTarget.Subscribe(newTarget => _lookTarget = newTarget));
            
            _weaponHandler.OnWeaponChanged += OnWeaponChanged;
        }

        private void OnWeaponChanged(WeaponConfig newWeapon)
        {
            switch (newWeapon.WeaponType)
            {
                case WeaponType.Melee:
                    _animator.SetBool(IS_MELEE_KEY, true);
                    _animator.SetBool(IS_PISTOL_KEY, false);
                    _animator.SetBool(IS_RIFLE_KEY, false);
                    break;
                
                case WeaponType.OneHand:
                    _animator.SetBool(IS_MELEE_KEY, false);
                    _animator.SetBool(IS_PISTOL_KEY, true);
                    _animator.SetBool(IS_RIFLE_KEY, false);
                    break;
                
                case WeaponType.TwoHand:
                    _animator.SetBool(IS_MELEE_KEY, false);
                    _animator.SetBool(IS_PISTOL_KEY, false);
                    _animator.SetBool(IS_RIFLE_KEY, true);
                    break;
            }
        }

        private void Update()
        {
            _animator.SetFloat(MOVE_SPEED_KEY, _playerMovement.Speed);

            if(_died == false)
                Rotate();
        }

        private void Rotate()
        {
            if (_lookTarget != null)
            {
                IsLookingToTarget = LookAt(_lookTarget.transform.position - transform.position, ROTATION_SPEED_TO_TARGET);
                return;
            }
            
            if (_playerMovement.IsMoving.CurrentValue)
                LookAt(_playerMovement.MoveDirection, ROTATION_SPEED_TO_MOVE_DIR);
            
            IsLookingToTarget = false;
        }
        
        private bool LookAt(Vector3 direction, float rotationSpeed, float angleOffset = 8f)
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
            
            _weaponHandler.OnWeaponChanged -= OnWeaponChanged;
        }
    }
}