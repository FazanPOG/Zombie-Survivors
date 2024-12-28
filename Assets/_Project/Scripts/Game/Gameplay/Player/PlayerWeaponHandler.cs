using UnityEngine;

namespace _Project.Gameplay
{
    public class PlayerWeaponHandler
    {
        private WeaponConfig _currentWeapon;
        private WeaponView _currentWeaponView;
        private readonly Transform _playerHand;

        public PlayerWeaponHandler(WeaponConfig startWeapon, Transform playerHand)
        {
            _currentWeapon = startWeapon;
            _playerHand = playerHand;

            EquipWeapon(startWeapon);
        }

        private void EquipWeapon(WeaponConfig newWeapon)
        {
            _currentWeapon = newWeapon;
            
            if (_currentWeaponView != null)
                Object.Destroy(_currentWeaponView.gameObject);

            var newWeaponView = Object.Instantiate(_currentWeapon.WeaponViewPrefab);
            newWeaponView.Attach(_playerHand);
        }
    }
}