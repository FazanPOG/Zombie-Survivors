using System;
using R3;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour, IDamageable, IPauseHandler
    {
        [SerializeField] private ParticleSystem _bloodFX;

        private readonly ReactiveProperty<int> _currentHealth = new ReactiveProperty<int>();
        
        private NavMeshAgent _navMeshAgent;
        private BoxCollider _collider;
        private ZombieType _zombieType;
        private ZombieMovement _movement;
        private ZombieAttacker _attacker;
        private ZombieView _view;
        private IScoreService _scoreService;
        private Action<IPauseHandler> _unregisterAction;
        private IZombieCounterService _zombieCounterService;

        public bool CanTakeDamage { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Init(ZombieConfig config, IScoreService scoreService, Action<IPauseHandler> unregisterAction, IZombieCounterService zombieCounterService)
        {
            _zombieCounterService = zombieCounterService;
            _unregisterAction = unregisterAction;
            _zombieType = config.ZombieType;
            _scoreService = scoreService;
            _currentHealth.Value = config.Health;
            CanTakeDamage = true;
            
            _movement = new ZombieMovement(_navMeshAgent, config.MoveSpeed);
            _attacker = new ZombieAttacker(transform, config.Damage);
            ZombieView randomViewPrefab = config.ViewPrefabs[Random.Range(0, config.ViewPrefabs.Length - 1)];
            _view = Instantiate(randomViewPrefab, transform);
            _view.Init(DestroySelf, config.MoveSpeed, _currentHealth, _bloodFX);
            
            EnableZombieLogic();
        }

        private void Update()
        {
            _movement.Update();
            _attacker.Update();
        }

        public void SetTarget(Player target)
        {
            _movement.SetMoveTarget(target.transform);
            _attacker.SetAttackTarget(target);
        }

        public void TakeDamage(int damage)
        {
            if(damage < 0)
                throw new Exception();
            
            _currentHealth.Value -= damage;

            if (_currentHealth.Value <= 0)
            {
                _currentHealth.Value = 0;
                Kill();
            }
        }

        public void Kill()
        {
            _scoreService.ZombieKilled(_zombieType);
            _zombieCounterService.Remove();
            _unregisterAction?.Invoke(this);
            DisableZombieLogic();
            _view.PlayDiedAnimation();
        }
        
        private void DestroySelf()
        {
            //TODO: pool.Release
            Destroy(gameObject);
        }

        public void HandlePause(bool isPaused)
        {
            if(isPaused)
                DisableZombieLogic();
            else
                EnableZombieLogic();
        }

        private void DisableZombieLogic()
        {
            _movement.StopMove();
            _attacker.StopAttack();
            CanTakeDamage = false;
            _collider.enabled = false;
        }
        
        private void EnableZombieLogic()
        {
            _movement.StartMove();
            _attacker.StartAttack();
            CanTakeDamage = true;
            _collider.enabled = true;
        }
    }
}