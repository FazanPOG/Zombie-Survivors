using System;
using System.Linq;
using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.MainMenu;

namespace _Project.UI
{
    public class UpgradeShopItemViewPresenter
    {
        private readonly UpgradeShopItemView _view;
        private readonly UpgradeItemConfig _itemConfig;
        private readonly IGameDataProvider _gameDataProvider;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly AudioPlayer _audioPlayer;

        private UpgradeShopItemData _data;
        private int _defaultPlayerHealth;
        private float _defaultPlayerMoveSpeed;
        
        public UpgradeShopItemViewPresenter(
            UpgradeShopItemView view, 
            UpgradeItemConfig itemConfig, 
            IGameDataProvider gameDataProvider,
            ILocalizationProvider localizationProvider,
            AudioPlayer audioPlayer)
        {
            _view = view;
            _itemConfig = itemConfig;
            _gameDataProvider = gameDataProvider;
            _localizationProvider = localizationProvider;
            _audioPlayer = audioPlayer;

            InitData();
            InitView();
            UpdateInfoTexts();
            
            _view.OnUpgradeButtonClicked += OnUpgradeButtonClicked;
        }

        private void OnUpgradeButtonClicked()
        {
            if (CanBuy())
            {
                Buy();
                _audioPlayer.PlayButtonClickSound();
            }
        }

        private bool CanBuy()
        {
            if (IsMaxLevel())
                return false;
            
            int price = CalculatePrice(_data.CurrentLevel);
            int softCurrencyAmount = _gameDataProvider.GameDataProxy.SoftCurrency.Value;
            int hardCurrencyAmount = _gameDataProvider.GameDataProxy.HardCurrency.Value;
            
            switch (_itemConfig.CurrencyType)
            {
                case CurrencyType.SoftCurrency:
                    return price <= softCurrencyAmount;
                
                case CurrencyType.HardCurrency:
                    return price <= hardCurrencyAmount;
                
                default:
                    throw new Exception();
            }
        }

        private void Buy()
        {
            int price = CalculatePrice(_data.CurrentLevel);
            
            switch (_itemConfig.CurrencyType)
            {
                case CurrencyType.SoftCurrency:
                    _gameDataProvider.GameDataProxy.SoftCurrency.Value -= price;
                    break;
                
                case CurrencyType.HardCurrency:
                    _gameDataProvider.GameDataProxy.HardCurrency.Value -= price;
                    break;
                
                default:
                    throw new Exception();
            }
            
            UpdateGameData();
            ApplyUpgrade();
            _gameDataProvider.SaveGameData();
            UpdateInfoTexts();
        }

        private void UpdateGameData()
        {
            var dataToRemove = _gameDataProvider.GameDataProxy.UpgradeItemDatas.First(x => x.ID == _itemConfig.ID);
            _gameDataProvider.GameDataProxy.UpgradeItemDatas.Remove(dataToRemove);
            _data.CurrentLevel++;
            _gameDataProvider.GameDataProxy.UpgradeItemDatas.Add(_data);
        }
        
        private void InitData()
        {
            var shopItemDatas = _gameDataProvider.GameDataProxy.UpgradeItemDatas;
            var data = shopItemDatas.FirstOrDefault(x => x.ID == _itemConfig.ID);

            if (data == null)
            {
                _data = CreateShopItemData();
                shopItemDatas.Add(_data);
                _gameDataProvider.SaveGameData();
            }
            else
            {
                _data = data;
            }

            _defaultPlayerHealth = _gameDataProvider.GameDataProxy.PlayerHealth.Value / _data.CurrentLevel;
            _defaultPlayerMoveSpeed = _gameDataProvider.GameDataProxy.PlayerMoveSpeed.Value / _data.CurrentLevel;
        }

        private void InitView()
        {
            _view.SetIcon(_itemConfig.ItemIcon);
            _view.SetIconBackground(_itemConfig.ItemIconBackgroundSprite);

            switch (_itemConfig.UpgradeType)
            {
                case UpgradeType.PlayerHealth:
                    _view.SetNameText(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.HP_KEY));
                    break;
                
                case UpgradeType.PlayerMoveSpeed:
                    _view.SetNameText(_localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.MOVE_SPEED_KEY));
                    break;
            }
        }

        private void UpdateInfoTexts()
        {
            bool isMaxLevel = IsMaxLevel();

            updateLevelTexts();
            updateValueTexts();
            updateButton();
            
            void updateLevelTexts()
            {
                _view.SetNextLevelArrowImageActiveState(!isMaxLevel);
                _view.SetNextLevelTextActiveState(!isMaxLevel);
                
                if (isMaxLevel)
                {
                    string text = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.MAX_KEY);
                    _view.SetCurrentLevelTexts(text);
                }
                else
                {
                    string levelText = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.LEVEL_SHORT_KEY);
                    _view.SetCurrentLevelTexts($"{levelText}.{_data.CurrentLevel}");
                    _view.SetNextLevelText($"{levelText}.{_data.CurrentLevel + 1}");
                }
            }
            
            void updateValueTexts()
            {
                _view.SetNextValueTextActiveState(!isMaxLevel);
                _view.SetNextValueArrowImageActiveState(!isMaxLevel);
                
                if (isMaxLevel)
                {
                    switch (_itemConfig.UpgradeType)
                    {
                        case UpgradeType.PlayerHealth:
                            _view.SetCurrentValueText($"{_gameDataProvider.GameDataProxy.PlayerHealth.Value}");
                            break;
                        
                        case UpgradeType.PlayerMoveSpeed:
                            _view.SetCurrentValueText($"{_gameDataProvider.GameDataProxy.PlayerMoveSpeed.Value}");
                            break;
                    }
                }
                else
                {
                    string firstPartValueText = "ERROR";
                    switch (_itemConfig.UpgradeType)
                    {
                        case UpgradeType.PlayerHealth:
                            firstPartValueText = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.HP_KEY);;
                            break;
                        
                        case UpgradeType.PlayerMoveSpeed:
                            firstPartValueText = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.MOVE_SPEED_KEY);;
                            break;
                    }
                    
                    _view.SetCurrentValueText($"{firstPartValueText} +{CalculateUpgradeValue(_data.CurrentLevel)}");
                    _view.SetNextValueText($"+{CalculateUpgradeValue(_data.CurrentLevel + 1)}");
                }
            }

            void updateButton()
            {
                _view.SetUpgradeButtonImageActiveState(!isMaxLevel);
                
                if (isMaxLevel)
                {
                    string text = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SOLD_OUT_KEY);;
                    _view.SetPriceText(text);
                }
                else
                {
                    _view.SetPriceText($"{CalculatePrice(_data.CurrentLevel)}");
                }
            }
        }
        
        private bool IsMaxLevel()
        {
            if(_data.CurrentLevel > _itemConfig.MaxUpgradeLevel)
                throw new Exception();
            
            return _data.CurrentLevel == _itemConfig.MaxUpgradeLevel;
        }

        private int CalculateUpgradeValue(int level)
        {
            float result;
            
            switch (_itemConfig.UpgradeType)
            {
                case UpgradeType.PlayerHealth:
                    result = _defaultPlayerHealth;
                    break;
                
                case UpgradeType.PlayerMoveSpeed:
                    result = _defaultPlayerMoveSpeed;
                    break;
                
                default:
                    throw new Exception();
            }
            
            for (int i = 1; i < level; i++)
                result *= _itemConfig.IncreaseUpgradeValueCoefficient;

            return (int)result;
        }

        private int CalculatePrice(int level)
        {
            float result = _itemConfig.DefaultPrice;
            
            for (int i = 1; i < level; i++)
                result *= _itemConfig.IncreasePriceCoefficient;

            return (int)result;
        }

        private void ApplyUpgrade()
        {
            switch (_itemConfig.UpgradeType)
            {
                case UpgradeType.PlayerHealth:
                    _gameDataProvider.GameDataProxy.PlayerHealth.Value = CalculateUpgradeValue(_data.CurrentLevel);
                    break;
                
                case UpgradeType.PlayerMoveSpeed:
                    _gameDataProvider.GameDataProxy.PlayerMoveSpeed.Value = CalculateUpgradeValue(_data.CurrentLevel);
                    break;
            }
        }
        
        private UpgradeShopItemData CreateShopItemData()
        {
            return new UpgradeShopItemData()
            {
                ID = _itemConfig.ID,
                CurrentLevel = 1,
            };
        }
    }
}