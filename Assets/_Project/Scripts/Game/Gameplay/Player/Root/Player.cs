using System;
using System.Collections;
using _Project.Audio;
using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerCollisionHandler _collisionHandler;
        [SerializeField] private PlayerView _view;
        [SerializeField] private Transform _hand;
        [SerializeField, TextArea(0, 10)] private string DEBUG_STRING; 
        
        private readonly ReactiveProperty<WeaponConfig> _currentWeapon = new ReactiveProperty<WeaponConfig>();
        private readonly ReactiveProperty<float> _attackRange = new ReactiveProperty<float>();
        
        private PlayerHealth _playerHealth;
        private PlayerMoveSpeed _playerMoveSpeed;
        private PlayerMovement _movement;
        private PlayerAttacker _playerAttacker;
        private PlayerWeaponHandler _playerWeaponHandler;
        private Transform _spawnPoint;

        public bool CanTakeDamage { get; private set; }

        public void Init(
            IInput input,
            PlayerHealth playerHealth, 
            PlayerMoveSpeed playerMoveSpeed,
            WeaponConfig startWeapon,
            AudioPlayer audioPlayer,
            Transform spawnPoint)
        {
            _playerMoveSpeed = playerMoveSpeed;
            _playerHealth = playerHealth;
            _spawnPoint = spawnPoint;
            _currentWeapon.Value = startWeapon;
            _attackRange.Value = startWeapon.AttackRange;
            CanTakeDamage = true;
            
            _movement = new PlayerMovement(transform, input, _playerMoveSpeed.MoveSpeed);
            _playerWeaponHandler = new PlayerWeaponHandler(_hand, audioPlayer);
            _playerAttacker = new PlayerAttacker(this, _currentWeapon, _collisionHandler, _playerWeaponHandler, _view);

            _collisionHandler.Init(_attackRange);
            _view.Init(_movement, _playerWeaponHandler, _playerHealth, _collisionHandler.ClosestEnemy, audioPlayer);
            
            _movement.EnableMovement();
            _playerAttacker.EnableAttack();
            _playerWeaponHandler.EquipWeapon(startWeapon);
            
            _currentWeapon.Skip(1).Subscribe(newWeapon => _attackRange.Value = newWeapon.AttackRange);
        }
        
        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new Exception();

            _playerHealth.Health.Value -= damage;

            if (_playerHealth.Health.Value <= 0)
            {
                _playerHealth.Health.Value = 0;
                CanTakeDamage = false;
                _movement.DisableMovement();
                _playerAttacker.DisableAttack();
            }
            else
            {
                StartCoroutine(InvincibleFrames());
            }
        }

        public void Revive()
        {
            Heal(_playerHealth.MaxHealth);
            CanTakeDamage = true;
            _movement.EnableMovement();
            _playerAttacker.EnableAttack();
            transform.position = _spawnPoint.transform.position;
        }
        
        private void Heal(int heal)
        {
            if (heal < 0)
                throw new Exception();

            _playerHealth.Health.Value += heal;

            if (_playerHealth.Health.Value > _playerHealth.MaxHealth)
                _playerHealth.Health.Value = _playerHealth.MaxHealth;
        }

        public void TakeBoost(BaseBoost boost)
        {
            switch (boost)
            {
                case SpeedBoost speedBoost:
                    _playerMoveSpeed.MoveSpeed.Value += speedBoost.Speed;
                    break;
                case HealBoost healBoost:
                    Heal(healBoost.HealAmount);
                    break;
                case WeaponBoost weaponBoost:
                    _playerWeaponHandler.EquipWeapon(weaponBoost.WeaponConfig);
                    break;
                
                default:
                    throw new NotImplementedException($"Boost does not implemented: {boost}");
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
            DEBUG_STRING += $"Health: {_playerHealth?.Health?.CurrentValue} \n" +
                            $"Is Movement Enabled: {_movement.IsEnabled} \n" +
                            $"Move speed: {_movement.Speed} \n " +
                            $"Can attack: {_playerAttacker.CanAttack}";
        }
    }
}