using _Project.Data;
using _Project.MainMenu;
using _Project.Root;
using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private PlayButtonView _playButtonView;
        [SerializeField] private PopupButtonView _settingsButtonView;
        [SerializeField] private PopupButtonView _upgradeShopButtonView;
        [Header("Screens")]
        [SerializeField] private ShopView shopView;
        [Header("Popups")]
        [SerializeField] private SettingsPopupView _settingsPopupView;
        [Header("Shop")]
        [SerializeField] private UpgradeItemConfig[] _upgradeItemConfigs;
        [SerializeField] private UpgradeShopItemView _upgradeItemViewPrefab;
        [SerializeField] private BulletItemConfig[] _bulletItemConfigs;
        [SerializeField] private BulletShopItemView _bulletItemViewPrefab;
        [SerializeField] private Transform _shopItemsContainer;
        [SerializeField] private Transform _bulletItemsContainer;
        
        private DiContainer _container;
        
        public void Bind(DiContainer diContainer)
        {
            _container = diContainer;

            BindPresenters();
        }

        private void BindPresenters()
        {
            var sceneLoader = _container.Resolve<ISceneLoaderService>();
            var gameDataProvider = _container.Resolve<IGameDataProvider>();
            
            new PlayButtonViewPresenter(_playButtonView, sceneLoader);
            new SettingsPopupViewPresenter(_settingsButtonView, _settingsPopupView, gameDataProvider);
            new ShopViewPresenter(
                shopView, 
                _upgradeShopButtonView, 
                _upgradeItemConfigs, 
                _upgradeItemViewPrefab, 
                _bulletItemConfigs,
                _bulletItemViewPrefab,
                _shopItemsContainer, 
                _bulletItemsContainer,
                gameDataProvider);
        }
    }
}