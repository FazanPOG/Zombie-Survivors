using _Project.Data;
using R3;

namespace _Project.UI
{
    public class CurrencyViewPresenter
    {
        private readonly CurrencyType _currencyType;
        private readonly CurrencyView _currencyView;
        private readonly IGameDataProvider _gameDataProvider;

        public CurrencyViewPresenter(CurrencyType currencyType, CurrencyView currencyView, IGameDataProvider gameDataProvider)
        {
            _currencyType = currencyType;
            _currencyView = currencyView;
            _gameDataProvider = gameDataProvider;

            Subscribe();
        }

        private void Subscribe()
        {
            switch (_currencyType)
            {
                case CurrencyType.SoftCurrency:
                    _gameDataProvider.GameDataProxy.SoftCurrency.Subscribe(UpdateText);
                    break;
                
                case CurrencyType.HardCurrency:
                    _gameDataProvider.GameDataProxy.HardCurrency.Subscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int currencyAmount)
        {
            _currencyView.SetCurrencyText(currencyAmount.ToString());
        }
    }
}