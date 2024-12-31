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
        [SerializeField] private UpgradeShopView _upgradeShopView;
        [Header("Popups")]
        [SerializeField] private SettingsPopupView _settingsPopupView;
        [Header("UpgradeShop")]
        [SerializeField] private UpgradeItemConfig[] _shopItemConfigs;
        [SerializeField] private UpgradeShopItemView _shopItemViewPrefab;
        [SerializeField] private Transform _shopItemsContainer;
        
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
            new UpgradeShopViewPresenter(_upgradeShopView, _upgradeShopButtonView, _shopItemConfigs, _shopItemViewPrefab, _shopItemsContainer, gameDataProvider);
        }
    }
}