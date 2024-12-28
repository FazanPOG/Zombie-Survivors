using UnityEngine;

namespace _Project.Gameplay
{
    public class ZombieAttacker
    {
        private readonly Transform _zombieTransform;
        private readonly Player _player;
        private readonly int _damage;

        private bool _canAttack;
        
        public ZombieAttacker(Transform zombieTransform, Player player, int damage)
        {
            _zombieTransform = zombieTransform;
            _player = player;
            _damage = damage;
            
            _canAttack = true;
        }

        public void StopAttack() => _canAttack = false;
        
        public void Update()
        {
            if (Vector3.Distance(_zombieTransform.position, _player.transform.position) < 0.4f)
            {
                if (_player.CanTakeDamage && _canAttack)
                    _player.TakeDamage(_damage);
            }
        }
    }
}