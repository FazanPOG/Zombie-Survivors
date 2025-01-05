using System;
using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.MainMenu;
using _Project.Root;
using _Project.Scripts.Game.Data;
using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private PlayButtonView _playButtonView;
        [SerializeField] private PopupButtonView _settingsButtonView;
        [SerializeField] private PopupButtonTextView _shopButtonView;
        [SerializeField] private CurrencyView[] _softCurrencyViews;
        [SerializeField] private CurrencyView[] _hardCurrencyViews;
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

            _container.Resolve<IAPIEnvironmentService>().GameLoadingAndReady();
        }

        private void BindPresenters()
        {
            var sceneLoader = _container.Resolve<ISceneLoaderService>();
            var gameDataProvider = _container.Resolve<IGameDataProvider>();
            var localizationProvider = _container.Resolve<ILocalizationProvider>();
            var adService = _container.Resolve<IADService>();
            var audioPlayer = _container.Resolve<AudioPlayer>();
            var defaultDataConfig = _container.Resolve<DefaultDataConfig>();

            foreach (var softCurrencyView in _softCurrencyViews)
                new CurrencyViewPresenter(CurrencyType.SoftCurrency, softCurrencyView, gameDataProvider);

            foreach (var hardCurrencyView in _hardCurrencyViews)
                new CurrencyViewPresenter(CurrencyType.HardCurrency, hardCurrencyView, gameDataProvider);


            new PlayButtonViewPresenter(_playButtonView, sceneLoader, localizationProvider);
            new SettingsPopupViewPresenter(_settingsButtonView, _settingsPopupView, gameDataProvider, localizationProvider, audioPlayer);
            new ShopViewPresenter(
                shopView, 
                _shopButtonView, 
                _upgradeItemConfigs, 
                _upgradeItemViewPrefab, 
                _bulletItemConfigs,
                _bulletItemViewPrefab,
                _shopItemsContainer, 
                _bulletItemsContainer,
                gameDataProvider,
                localizationProvider,
                adService,
                audioPlayer,
                defaultDataConfig);
        }
    }
}