using _Project.Game;
using UnityEngine;

namespace _Project.Gameplay
{
    public class PlayerWeaponHandler
    {
        private readonly Transform _playerHand;
        private readonly AudioPlayer _audioPlayer;

        private WeaponConfig _currentWeapon;
        private WeaponView _currentWeaponView;

        public WeaponView WeaponView => _currentWeaponView;
        
        public PlayerWeaponHandler(WeaponConfig startWeapon, Transform playerHand, AudioPlayer audioPlayer)
        {
            _currentWeapon = startWeapon;
            _playerHand = playerHand;
            _audioPlayer = audioPlayer;

            EquipWeapon(startWeapon);
        }

        private void EquipWeapon(WeaponConfig newWeapon)
        {
            _currentWeapon = newWeapon;
            
            if (_currentWeaponView != null)
                Object.Destroy(_currentWeaponView.gameObject);

            _currentWeaponView = Object.Instantiate(_currentWeapon.WeaponViewPrefab);
            _currentWeaponView.Init(_currentWeapon, _audioPlayer, _playerHand);
        }
    }
}