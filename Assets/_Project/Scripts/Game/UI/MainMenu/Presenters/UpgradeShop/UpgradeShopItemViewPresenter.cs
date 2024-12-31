using System;
using System.Linq;
using _Project.Data;
using _Project.MainMenu;

namespace _Project.UI
{
    public class UpgradeShopItemViewPresenter
    {
        private readonly UpgradeShopItemView _view;
        private readonly UpgradeItemConfig _itemConfig;
        private readonly IGameDataProvider _gameDataProvider;

        private UpgradeShopItemData _data;
        private int _defaultPlayerHealth;
        private float _defaultPlayerMoveSpeed;
        
        public UpgradeShopItemViewPresenter(
            UpgradeShopItemView view, 
            UpgradeItemConfig itemConfig, 
            IGameDataProvider gameDataProvider)
        {
            _view = view;
            _itemConfig = itemConfig;
            _gameDataProvider = gameDataProvider;
            
            InitData();
            InitView();
            UpdateInfoTexts();
            
            _view.OnUpgradeButtonClicked += OnUpgradeButtonClicked;
        }

        private void OnUpgradeButtonClicked()
        {
            if(CanBuy())
                Buy();
        }

        private bool CanBuy()
        {
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
            UpdateGameData();
            ApplyUpgrade();
            _gameDataProvider.SaveGameData();
            UpdateInfoTexts();
        }

        private void UpdateGameData()
        {
            var dataToRemove = _gameDataProvider.GameDataProxy.ShopItemDatas.First(x => x.ID == _itemConfig.ID);
            _gameDataProvider.GameDataProxy.ShopItemDatas.Remove(dataToRemove);
            _data.CurrentLevel++;
            _gameDataProvider.GameDataProxy.ShopItemDatas.Add(_data);
        }
        
        private void InitData()
        {
            var shopItemDatas = _gameDataProvider.GameDataProxy.ShopItemDatas;
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
            _view.SetNameText(_itemConfig.ItemNameText);
        }

        //TODO: localize
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
                    _view.SetCurrentLevelTexts($"MAX");
                }
                else
                {
                    _view.SetCurrentLevelTexts($"LV.{_data.CurrentLevel}");
                    _view.SetNextLevelText($"LV.{_data.CurrentLevel + 1}");
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
                            firstPartValueText = "HP";
                            break;
                        
                        case UpgradeType.PlayerMoveSpeed:
                            firstPartValueText = "Move Speed";
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
                    _view.SetPriceText("Sold out");
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