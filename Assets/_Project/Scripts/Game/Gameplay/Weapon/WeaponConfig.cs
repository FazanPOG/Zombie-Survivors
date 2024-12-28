using UnityEngine;

namespace _Project.Gameplay
{
    [CreateAssetMenu(menuName = "_Project/Gameplay/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField, Range(1, 50)] private int _damage = 1;
        [SerializeField, Range(0.1f, 50f)] private float _attackRange = 1f;
        [SerializeField, Range(0.1f, 50f)] private float _fireRate = 1f;
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private AudioClip _shotSFX;

        public WeaponType WeaponType => _weaponType;

        public int Damage => _damage;

        public float AttackRange => _attackRange;
        
        public float FireRate => _fireRate;

        public WeaponView WeaponViewPrefab => _weaponViewPrefab;

        public AudioClip ShotSFX => _shotSFX;
    }
}