using System;
using R3;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour, IDamageable
    {
        [SerializeField] private ZombieConfig _config;
        [SerializeField] private ParticleSystem _bloodFX;
        
        private NavMeshAgent _navMeshAgent;
        private BoxCollider _collider;
        private ZombieMovement _movement;
        private ZombieAttacker _attacker;
        private ZombieView _view;
        private ReactiveProperty<int> _currentHealth = new ReactiveProperty<int>();

        public bool CanTakeDamage { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Init(Player player)
        {
            _currentHealth.Value = _config.Health;
            CanTakeDamage = true;
            
            _movement = new ZombieMovement(_navMeshAgent, player, _config.MoveSpeed);
            _attacker = new ZombieAttacker(transform, player, _config.Damage);
            ZombieView randomViewPrefab = _config.ViewPrefabs[Random.Range(0, _config.ViewPrefabs.Length - 1)];
            _view = Instantiate(randomViewPrefab, transform);
            _view.Init(Died, _config.MoveSpeed, _currentHealth, _bloodFX);
        }

        private void Update()
        {
            _movement.Update();
            _attacker.Update();
        }

        public void TakeDamage(int damage)
        {
            if(damage < 0)
                throw new Exception();
            
            _currentHealth.Value -= damage;

            if (_currentHealth.Value <= 0)
            {
                _currentHealth.Value = 0;
                DisableZombie();
            }
        }

        private void DisableZombie()
        {
            _view.PlayDiedAnimation();
            _movement.StopMove();
            _attacker.StopAttack();
            CanTakeDamage = false;
            _collider.enabled = false;
        }
        
        private void Died()
        {
            //TODO: pool.Release
            Destroy(gameObject);
        }
    }
}