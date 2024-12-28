using System;
using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private readonly ReactiveProperty<Zombie> _closestEnemy = new ReactiveProperty<Zombie>();
        
        private SphereCollider _collider;
        private float _attackRange;
        private IDisposable _disposable;

        public ReadOnlyReactiveProperty<Zombie> ClosestEnemy => _closestEnemy;

        private void Awake() => _collider = GetComponent<SphereCollider>();

        public void Init(ReadOnlyReactiveProperty<float> attackRange)
        {
            _attackRange = attackRange.CurrentValue;
            _collider.radius = _attackRange;

            _disposable = attackRange.Subscribe(newValue => _attackRange = newValue);
        }

        private void FixedUpdate()
        {
            var colliders = Physics.OverlapSphere(transform.position, _attackRange);

            Zombie closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Zombie zombie))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        
                        if(zombie.CanTakeDamage)
                            closestEnemy = zombie;
                    }
                }
            }

            _closestEnemy.Value = closestEnemy;
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}