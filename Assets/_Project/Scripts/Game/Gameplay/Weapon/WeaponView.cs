using System;
using _Project.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Gameplay
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField, ShowIf(nameof(_isShootingWeapon))] private ParticleSystem _muzzleFlashFX;

        private WeaponConfig _weaponConfig;
        private AudioPlayer _audioPlayer;
        private bool _isShootingWeapon;

        private void Awake()
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
            
            _muzzleFlashFX.Stop();
        }

        public void Init(WeaponConfig weaponConfig, AudioPlayer audioPlayer, Transform parent)
        {
            _weaponConfig = weaponConfig;
            _audioPlayer = audioPlayer;
            
            transform.SetParent(parent, false);
        }

        public void Shoot()
        {
            if(_isShootingWeapon == false)
                throw new ArgumentException($"Attempt to shoot a melee weapon: {gameObject}");
            
            _muzzleFlashFX.Play();
            _audioPlayer.PlaySoundOneShot(_weaponConfig.ShotSFX);
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