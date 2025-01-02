using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.MainMenu;
using UnityEngine;

namespace _Project.UI
{
    public class ShopViewPresenter
    {
        private readonly ShopView _view;
        private readonly PopupButtonTextView _popupButtonView;
        private readonly UpgradeItemConfig[] _upgradeItemConfigs;
        private readonly UpgradeShopItemView _upgradeShopItemPrefab;
        private readonly BulletItemConfig[] _bulletItemConfigs;
        private readonly BulletShopItemView _bulletShopItemViewPrefab;
        private readonly Transform _upgradeItemsViewContainer;
        private readonly Transform _bulletItemsViewContainer;
        private readonly IGameDataProvider _gameDataProvider;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly IADService _adService;
        private readonly AudioPlayer _audioPlayer;

        public ShopViewPresenter(
            ShopView view, 
            PopupButtonTextView popupButtonView, 
            UpgradeItemConfig[] upgradeItemConfigs,
            UpgradeShopItemView upgradeShopItemPrefab,
            BulletItemConfig[] bulletItemConfigs,
            BulletShopItemView bulletShopItemViewPrefab,
            Transform upgradeItemsViewContainer,
            Transform bulletItemsViewContainer,
            IGameDataProvider gameDataProvider,
            ILocalizationProvider localizationProvider,
            IADService adService,
            AudioPlayer audioPlayer)
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
            _localizationProvider = localizationProvider;
            _adService = adService;
            _audioPlayer = audioPlayer;

            _popupButtonView.SetText(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SHOP_KEY));
            _view.SetShopTitleText(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SHOP_KEY));
            _view.SetBulletButtonTexts(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.BULLET_KEY));
            _view.SetUpgradeButtonTexts(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.UPGRADE_KEY));
            
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
            _audioPlayer.PlayButtonClickSound();
        }

        private void SetBulletShopView()
        {
            _view.SetBulletPanelActiveState(true);
            _view.SetUpgradePanelActiveState(false);
            _audioPlayer.PlayButtonClickSound();
        }

        private void InitItems()
        {
            foreach (var config in _upgradeItemConfigs)
            {
                var view = Object.Instantiate(_upgradeShopItemPrefab, _upgradeItemsViewContainer);
                new UpgradeShopItemViewPresenter(view, config, _gameDataProvider, _localizationProvider, _audioPlayer);
            }

            foreach (var config in _bulletItemConfigs)
            {
                var view = Object.Instantiate(_bulletShopItemViewPrefab, _bulletItemsViewContainer);
                new BulletShopItemViewPresenter(config, view, _gameDataProvider, _adService);
            }
        }

        private void PopupButtonViewOnButtonClicked()
        {
            _view.Show();
            _audioPlayer.PlayButtonClickSound();
        }

        private void OnCloseButtonClicked()
        {
            _audioPlayer.PlayButtonClickSound();
            _view.Hide();
        }
    }
}