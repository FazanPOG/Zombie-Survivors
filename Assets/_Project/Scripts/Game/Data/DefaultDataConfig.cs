using UnityEngine;

namespace _Project.Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "_Project/Data/DefaultDataConfig")]
    public class DefaultDataConfig : ScriptableObject
    {
        [Header("Player Data")]
        [SerializeField, Min(1)] private int _health = 1;
        [SerializeField, Range(0.1f, 20f)] private float _moveSpeed = 1f;
        [SerializeField, Range(0.1f, 20f)] private float _attackRange = 1f;
        
        public int Health => _health;
        public float PlayerMoveSpeed => _moveSpeed;
        public float PlayerAttackRange => _attackRange;
    }
}