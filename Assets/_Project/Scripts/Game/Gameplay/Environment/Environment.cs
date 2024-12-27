using UnityEngine;

namespace _Project.Gameplay
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        public Transform[] SpawnPoints => _spawnPoints;
    }
}
