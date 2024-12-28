using UnityEngine;

namespace _Project.Gameplay
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private Transform[] _playerSpawnPoints;
        [SerializeField] private Transform[] _zombieSpawnPoints;

        public Transform[] PlayerSpawnPoints => _playerSpawnPoints;
        public Transform[] ZombieSpawnPoints => _zombieSpawnPoints;
    }
}
