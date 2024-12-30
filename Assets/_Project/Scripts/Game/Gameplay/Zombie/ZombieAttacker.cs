using UnityEngine;

namespace _Project.Gameplay
{
    public class ZombieAttacker
    {
        private readonly Transform _zombieTransform;
        private readonly int _damage;

        private Player _player;
        private bool _canAttack;
        
        public ZombieAttacker(Transform zombieTransform, int damage)
        {
            _zombieTransform = zombieTransform;
            _damage = damage;
        }

        public void StopAttack() => _canAttack = false;
        public void StartAttack() => _canAttack = true;

        public void SetAttackTarget(Player player) => _player = player;
        
        public void Update()
        {
            if(_player == null)
                return;
            
            if (Vector3.Distance(_zombieTransform.position, _player.transform.position) < 1.25f)
            {
                if (_player.CanTakeDamage && _canAttack)
                    _player.TakeDamage(_damage);
            }
        }
    }
}