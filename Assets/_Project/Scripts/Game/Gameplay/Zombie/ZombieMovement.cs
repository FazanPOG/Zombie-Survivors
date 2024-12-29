using UnityEngine;
using UnityEngine.AI;

namespace _Project.Gameplay
{
    public class ZombieMovement
    {
        private readonly NavMeshAgent _navMeshAgent;

        private Transform _moveTarget;
        
        public ZombieMovement(NavMeshAgent navMeshAgent, float moveSpeed)
        {
            _navMeshAgent = navMeshAgent;

            _navMeshAgent.speed = moveSpeed;
        }

        public void SetMoveTarget(Transform target) => _moveTarget = target;
        
        public void StopMove() => _navMeshAgent.isStopped = true;

        public void Update()
        {
            if(_moveTarget != null)
                _navMeshAgent.destination = _moveTarget.position;
        }
    }
}