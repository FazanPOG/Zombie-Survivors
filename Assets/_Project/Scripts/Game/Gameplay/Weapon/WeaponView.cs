using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Gameplay
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField, ShowIf(nameof(_isShootingWeapon))] private ParticleSystem _muzzleFlash;

        private bool _isShootingWeapon;
        
        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void Shoot()
        {
            if(_isShootingWeapon == false)
                throw new ArgumentException($"Attempt to shoot a melee weapon: {gameObject}");
            
            _muzzleFlash?.Play();
        }
        
        private void OnValidate()
        {
            switch (_weaponType)
            {
                case WeaponType.None:
                    _isShootingWeapon = false;
                    break;
                
                case WeaponType.Melee:
                    _isShootingWeapon = false;
                    break;
                
                default:
                    _isShootingWeapon = true;
                    break;
            }
        }
    }
}