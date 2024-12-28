using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    public class Zombie : MonoBehaviour, IDamageable
    {
        [SerializeField] private ZombieConfig _config;

        private BoxCollider _collider;
        private ZombieView _view;
        private int _currentHealth;

        public bool CanTakeDamage { get; private set; }

        private void Awake() => _collider = GetComponent<BoxCollider>();

        public void Init()
        {
            _currentHealth = _config.Health;
            CanTakeDamage = true;
            
            ZombieMovement zombieMovement = new ZombieMovement();
            ZombieView randomViewPrefab = _config.ViewPrefabs[Random.Range(0, _config.ViewPrefabs.Length - 1)];
            _view = Instantiate(randomViewPrefab, transform);
            _view.Init(Died, _config.MoveSpeed);
        }

        public void TakeDamage(int damage)
        {
            if(damage < 0)
                throw new Exception();
            
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _view.PlayDiedAnimation();
                CanTakeDamage = false;
                _collider.enabled = false;
            }
        }

        private void Died()
        {
            //TODO: pool.Release
            Destroy(gameObject);
        }
    }
}