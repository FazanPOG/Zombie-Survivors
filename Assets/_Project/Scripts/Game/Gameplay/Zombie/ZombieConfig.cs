using UnityEngine;

namespace _Project.Gameplay
{
    [CreateAssetMenu(menuName = "_Project/Gameplay/ZombieConfig")]
    public class ZombieConfig : ScriptableObject
    {
        [SerializeField, Min(1)] private int _health;
        [SerializeField, Range(0.01f, 20f)] private float _moveSpeed = 1f;
        [SerializeField, Range(1, 50)] private int _damage = 1;
        [SerializeField] private ZombieView[] _viewPrefabs;

        public int Health => _health;

        public float MoveSpeed => _moveSpeed;

        public int Damage => _damage;
        
        public ZombieView[] ViewPrefabs => _viewPrefabs;
    }
}