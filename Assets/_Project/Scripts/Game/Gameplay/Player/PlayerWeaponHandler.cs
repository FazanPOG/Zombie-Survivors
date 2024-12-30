using System;
using _Project.Audio;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Gameplay
{
    public class PlayerWeaponHandler
    {
        private readonly Transform _playerHand;
        private readonly AudioPlayer _audioPlayer;

        private WeaponConfig _currentWeapon;
        private WeaponView _currentWeaponView;

        public WeaponView WeaponView => _currentWeaponView;

        public event Action<WeaponConfig> OnWeaponChanged; 

        public PlayerWeaponHandler(Transform playerHand, AudioPlayer audioPlayer)
        {
            _playerHand = playerHand;
            _audioPlayer = audioPlayer;
        }

        public void EquipWeapon(WeaponConfig newWeapon)
        {
            _currentWeapon = newWeapon;
            
            if (_currentWeaponView != null)
                Object.Destroy(_currentWeaponView.gameObject);

            _currentWeaponView = Object.Instantiate(_currentWeapon.WeaponViewPrefab);
            _currentWeaponView.Init(_currentWeapon, _audioPlayer, _playerHand);
            
            OnWeaponChanged?.Invoke(newWeapon);
        }
    }
}