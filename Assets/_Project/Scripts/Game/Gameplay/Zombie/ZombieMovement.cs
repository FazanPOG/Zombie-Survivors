using UnityEngine.AI;

namespace _Project.Gameplay
{
    public class ZombieMovement
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Player _player;
        
        public ZombieMovement(NavMeshAgent navMeshAgent, Player player, float moveSpeed)
        {
            _navMeshAgent = navMeshAgent;
            _player = player;

            _navMeshAgent.speed = moveSpeed;
        }

        public void StopMove() => _navMeshAgent.isStopped = true;

        public void Update()
        {
            _navMeshAgent.destination = _player.transform.position;
        }
    }
}