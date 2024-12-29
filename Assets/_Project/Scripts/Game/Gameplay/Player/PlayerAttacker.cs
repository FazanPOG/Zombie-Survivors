using System.Collections;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class PlayerAttacker
    {
        private MonoBehaviour _context;
        private readonly PlayerWeaponHandler _playerWeaponHandler;
        private readonly PlayerView _playerView;
        private float _attackDelay;
        private int _damage;
        private IDamageable _enemy;
        private bool _canAttack;
        
        public PlayerAttacker(
            MonoBehaviour context, 
            ReadOnlyReactiveProperty<WeaponConfig> weapon, 
            PlayerCollisionHandler playerCollisionHandler,
            PlayerWeaponHandler playerWeaponHandler,
            PlayerView playerView)
        {
            _context = context;
            _playerWeaponHandler = playerWeaponHandler;
            _playerView = playerView;
            _canAttack = true;
            
            weapon.Subscribe(newWeapon =>
            {
                _attackDelay = 1 / newWeapon.FireRate;
                _damage = newWeapon.Damage;
            });
            
            playerCollisionHandler.ClosestEnemy.Subscribe(enemy =>
            {
                if (enemy != null)
                {
                    if (enemy.TryGetComponent(out IDamageable damageable))
                        _enemy = damageable;
                    else
                        throw new MissingComponentException($"Missing {nameof(IDamageable)} component on: {nameof(enemy)}");
                }
            });
        }

        public void Update()
        {
            if(_enemy == null)
                return;
            
            if (_enemy.CanTakeDamage && _canAttack && _playerView.IsLookingToTarget)
            {
                _enemy.TakeDamage(_damage);
                _playerWeaponHandler.WeaponView.Shoot();
                _context.StartCoroutine(AttackCooldown());
            }
        }

        private IEnumerator AttackCooldown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(_attackDelay);
            _canAttack = true;
        }
    }
}