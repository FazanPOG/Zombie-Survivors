using UnityEngine;

namespace _Project.Gameplay
{
    [CreateAssetMenu(menuName = "_Project/Gameplay/Environment")]
    public class EnvironmentConfig : ScriptableObject
    {
        [Header("Root")]
        [SerializeField] private string _id;
        [Header("Zombie")]
        [SerializeField, Range(0f, 100f)] private float _zombieSpawnDelayMin;
        [SerializeField, Range(0f, 100f)] private float _zombieSpawnDelayMax;
        [SerializeField, Range(0, 1000)] private int _maxZombies;
        [Header("Boost")]
        [SerializeField, Range(0f, 100f)] private float _boostSpawnDelayMin;
        [SerializeField, Range(0f, 100f)] private float _boostSpawnDelayMax;
        [SerializeField, Range(0, 100)] private int _maxBoosts;

        public string ID => _id;
        
        public float ZombieSpawnDelayMin => _zombieSpawnDelayMin;
        public float ZombieSpawnDelayMax => _zombieSpawnDelayMax;

        public int MaxZombies => _maxZombies;
        public float BoostSpawnDelayMin => _boostSpawnDelayMin;
        public float BoostSpawnDelayMax => _boostSpawnDelayMax;
        public int MaxBoosts => _maxBoosts;
        
        private void OnValidate()
        {
            if (_zombieSpawnDelayMin > _zombieSpawnDelayMax)
                _zombieSpawnDelayMax = _zombieSpawnDelayMin;
            
            if (_boostSpawnDelayMin > _boostSpawnDelayMax)
                _boostSpawnDelayMax = _boostSpawnDelayMin;
        }
    }
}