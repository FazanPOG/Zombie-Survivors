using UnityEngine;

namespace _Project.Gameplay
{
    [CreateAssetMenu(menuName = "_Project/Gameplay/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField, Range(0.1f, 50f)] private float _damage = 1f;
        [SerializeField, Range(0.1f, 50f)] private float _fireRate = 1f;
        [SerializeField] private WeaponView _weaponViewPrefab;

        public WeaponType WeaponType => _weaponType;

        public float Damage => _damage;

        public float FireRate => _fireRate;

        public WeaponView WeaponViewPrefab => _weaponViewPrefab;
    }
}