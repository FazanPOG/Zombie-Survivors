using System;
using System.Collections;
using _Project.Data;
using _Project.Game;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerCollisionHandler _collisionHandler;
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _hand;
        [SerializeField] private WeaponConfig _testWeapon; //TODO
        [SerializeField, TextArea(0, 10)] private string DEBUG_STRING; 
        
        private readonly ReactiveProperty<WeaponConfig> _currentWeapon = new ReactiveProperty<WeaponConfig>();
        private readonly ReactiveProperty<float> _attackRange = new ReactiveProperty<float>();
        
        private PlayerHealth _playerHealth;
        private PlayerMovement _movement;
        private PlayerAttacker _playerAttacker;
        private int _maxHealth;

        public bool CanTakeDamage { get; private set; }

        public void Init(
            IInput input,
            IGameStateProvider gameStateProvider,
            PlayerHealth playerHealth, 
            PlayerMoveSpeed playerMoveSpeed, 
            AudioPlayer audioPlayer)
        {
            _playerHealth = playerHealth;
            _currentWeapon.Value = _testWeapon;
            _attackRange.Value = _testWeapon.AttackRange;
            CanTakeDamage = true;

            _collisionHandler.Init(_attackRange);
            _movement = new PlayerMovement(transform, input, playerMoveSpeed.MoveSpeed);
            var weaponHandler = new PlayerWeaponHandler(_testWeapon, _hand, audioPlayer);
            _playerAttacker = new PlayerAttacker(this, _currentWeapon, _collisionHandler, weaponHandler);
            _view.Init(_movement, _playerHealth.Health, _collisionHandler.ClosestEnemy);

            _currentWeapon.Skip(1).Subscribe(newWeapon => _attackRange.Value = newWeapon.AttackRange);
            
            gameStateProvider.GameState.Subscribe(state =>
            {
                if (state is GameplayState)
                {
                    _movement.EnableMovement();
                }
            });
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new Exception();

            _playerHealth.Health.Value -= damage;

            if (_playerHealth.Health.Value <= 0)
            {
                CanTakeDamage = false;
                _movement.DisableMovement();
            }
            else
            {
                StartCoroutine(InvincibleFrames());
            }
        }

        private void Update()
        {
            UpdateDebugString();
            _movement.Update();
            _playerAttacker.Update();
        }

        private IEnumerator InvincibleFrames()
        {
            CanTakeDamage = false;
            yield return new WaitForSeconds(1f);
            CanTakeDamage = true;
        }

        private void UpdateDebugString()
        {
            DEBUG_STRING = String.Empty;
            DEBUG_STRING += $"Is Movement Enabled: {_movement.IsEnabled} \n" +
                            $"Move speed: {_movement.Speed}";
        }
    }
}