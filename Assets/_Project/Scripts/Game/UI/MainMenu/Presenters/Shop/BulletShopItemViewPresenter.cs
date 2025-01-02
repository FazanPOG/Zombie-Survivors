using System;
using System.Linq;
using _Project.Data;
using _Project.MainMenu;

namespace _Project.UI
{
    public class BulletShopItemViewPresenter
    {
        private readonly BulletItemConfig _config;
        private readonly BulletShopItemView _view;
        private readonly IGameDataProvider _gameDataProvider;

        private BulletShopItemData _data;
        
        public BulletShopItemViewPresenter(
            BulletItemConfig config, 
            BulletShopItemView view,
            IGameDataProvider gameDataProvider)
        {
            _config = config;
            _view = view;
            _gameDataProvider = gameDataProvider;

            InitData();
            InitView();
            
            _view.OnBuyButtonClicked += OnBuyButtonClicked;
        }

        private void OnBuyButtonClicked()
        {
            if(CanBuy())
                Buy();
        }

        private void Buy()
        {
            UpdateGameData();
            UpdateBalance();
            UpdateADWatchedAmountText();
            _gameDataProvider.SaveGameData();
        }
        
        private bool CanBuy()
        {
            int price = _config.SoftCurrencyPrice;
            int softCurrencyAmount = _gameDataProvider.GameDataProxy.SoftCurrency.Value;
            
            switch (_config.PriceType)
            {
                case BulletItemPriceType.AD:
                    return true;
                
                case BulletItemPriceType.SoftCurrency:
                    return price <= softCurrencyAmount;

                default:
                    throw new Exception();
            }
        }

        private void UpdateBalance()
        {
            switch (_config.PriceType)
            {
                case BulletItemPriceType.AD:
                    if(_data.ADWatchedCount == 0)
                        _gameDataProvider.GameDataProxy.HardCurrency.Value += _config.BulletAmount;
                    break;
                
                case BulletItemPriceType.SoftCurrency:
                    _gameDataProvider.GameDataProxy.HardCurrency.Value += _config.BulletAmount;
                    break;
            }
        }
        
        private void InitData()
        {
            var bulletItemData = _gameDataProvider.GameDataProxy.BulletItemDatas;
            var data = bulletItemData.FirstOrDefault(x => x.ID == _config.ID);

            if (data == null)
            {
                _data = CreateBulletItemData();
                bulletItemData.Add(_data);
                _gameDataProvider.SaveGameData();
            }
            else
            {
                _data = data;
            }
        }
        
        private void UpdateGameData()
        {
            var dataToRemove = _gameDataProvider.GameDataProxy.BulletItemDatas.First(x => x.ID == _config.ID);
            _gameDataProvider.GameDataProxy.BulletItemDatas.Remove(dataToRemove);
            _data.ADWatchedCount++;

            if (_data.ADWatchedCount == _config.ADAmount)
                _data.ADWatchedCount = 0;
            
            _gameDataProvider.GameDataProxy.BulletItemDatas.Add(_data);
        }
        
        private void InitView()
        {
            _view.SetIconImageSprite(_config.IconSprite);
            _view.SetAmountText(_config.BulletAmount.ToString());

            switch (_config.PriceType)
            {
                case BulletItemPriceType.AD:
                    _view.SetADPriceImage();
                    UpdateADWatchedAmountText();
                    break;
                
                case BulletItemPriceType.SoftCurrency:
                    _view.SetSoftCurrencyPriceImage();
                    _view.SetPriceText(_config.SoftCurrencyPrice.ToString());
                    break;
            }
        }

        private void UpdateADWatchedAmountText()
        {
            _view.SetPriceText($"{_data.ADWatchedCount}/{_config.ADAmount}");
        }
        
        private BulletShopItemData CreateBulletItemData()
        {
            return new BulletShopItemData()
            {
                ID = _config.ID,
                ADWatchedCount = 0,
            };
        }
    }
}