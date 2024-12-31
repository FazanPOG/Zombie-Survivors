using UnityEngine;

namespace _Project.Gameplay
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private EnvironmentConfig _config;
        [SerializeField] private Transform[] _playerSpawnPoints;
        [SerializeField] private Transform[] _zombieSpawnPoints;
        [SerializeField] private Transform[] _boostSpawnPoints;

        public EnvironmentConfig Config => _config;
        
        public Transform[] PlayerSpawnPoints => _playerSpawnPoints;
        public Transform[] ZombieSpawnPoints => _zombieSpawnPoints;
        public Transform[] BoostSpawnPoints => _boostSpawnPoints;
    }
}
