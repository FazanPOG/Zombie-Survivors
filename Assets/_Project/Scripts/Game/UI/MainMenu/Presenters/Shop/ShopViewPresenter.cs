using _Project.Data;
using _Project.MainMenu;
using UnityEngine;

namespace _Project.UI
{
    public class ShopViewPresenter
    {
        private readonly ShopView _view;
        private readonly PopupButtonView _popupButtonView;
        private readonly UpgradeItemConfig[] _upgradeItemConfigs;
        private readonly UpgradeShopItemView _upgradeShopItemPrefab;
        private readonly BulletItemConfig[] _bulletItemConfigs;
        private readonly BulletShopItemView _bulletShopItemViewPrefab;
        private readonly Transform _upgradeItemsViewContainer;
        private readonly Transform _bulletItemsViewContainer;
        private readonly IGameDataProvider _gameDataProvider;

        public ShopViewPresenter(
            ShopView view, 
            PopupButtonView popupButtonView, 
            UpgradeItemConfig[] upgradeItemConfigs,
            UpgradeShopItemView upgradeShopItemPrefab,
            BulletItemConfig[] bulletItemConfigs,
            BulletShopItemView bulletShopItemViewPrefab,
            Transform upgradeItemsViewContainer,
            Transform bulletItemsViewContainer,
            IGameDataProvider gameDataProvider)
        {
            _view = view;
            _popupButtonView = popupButtonView;
            _upgradeItemConfigs = upgradeItemConfigs;
            _upgradeShopItemPrefab = upgradeShopItemPrefab;
            _bulletItemConfigs = bulletItemConfigs;
            _bulletShopItemViewPrefab = bulletShopItemViewPrefab;
            _upgradeItemsViewContainer = upgradeItemsViewContainer;
            _bulletItemsViewContainer = bulletItemsViewContainer;
            _gameDataProvider = gameDataProvider;

            _popupButtonView.OnButtonClicked += PopupButtonViewOnButtonClicked;
            _view.OnCloseButtonClicked += OnCloseButtonClicked;
            _view.OnBulletButtonClicked += SetBulletShopView;
            _view.OnUpgradeButtonClicked += SetUpgradeShopView;
            
            SetBulletShopView();
            
            InitItems();
        }

        private void SetUpgradeShopView()
        {
            _view.SetUpgradePanelActiveState(true);
            _view.SetBulletPanelActiveState(false);
        }

        private void SetBulletShopView()
        {
            _view.SetBulletPanelActiveState(true);
            _view.SetUpgradePanelActiveState(false);
        }

        private void InitItems()
        {
            foreach (var config in _upgradeItemConfigs)
            {
                var view = Object.Instantiate(_upgradeShopItemPrefab, _upgradeItemsViewContainer);
                new UpgradeShopItemViewPresenter(view, config, _gameDataProvider);
            }

            foreach (var config in _bulletItemConfigs)
            {
                var view = Object.Instantiate(_bulletShopItemViewPrefab, _bulletItemsViewContainer);
                new BulletShopItemViewPresenter(config, view, _gameDataProvider);
            }
        }

        private void PopupButtonViewOnButtonClicked()
        {
            _view.Show();
        }

        private void OnCloseButtonClicked()
        {
            _view.Hide();
        }
    }
}