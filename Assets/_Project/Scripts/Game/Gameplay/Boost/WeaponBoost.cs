using UnityEngine;

namespace _Project.Gameplay
{
    public class WeaponBoost : BaseBoost
    {
        [SerializeField] private Transform _weaponViewParent;

        public WeaponConfig WeaponConfig { get; private set; }

        public void Init(WeaponConfig weaponConfig)
        {
            WeaponConfig = weaponConfig;
            
            Instantiate(WeaponConfig.WeaponViewPrefab, _weaponViewParent.transform.position, Quaternion.identity, _weaponViewParent);
        }
    }
}