using _Project.Data;
using _Project.MainMenu;
using UnityEngine;

namespace _Project.UI
{
    public class UpgradeShopViewPresenter
    {
        private readonly UpgradeShopView _view;
        private readonly PopupButtonView _popupButtonView;
        private readonly UpgradeItemConfig[] _itemConfigs;
        private readonly UpgradeShopItemView _itemPrefab;
        private readonly Transform _itemViewContainer;
        private readonly IGameDataProvider _gameDataProvider;

        public UpgradeShopViewPresenter(
            UpgradeShopView view, 
            PopupButtonView popupButtonView, 
            UpgradeItemConfig[] itemConfigs,
            UpgradeShopItemView itemPrefab,
            Transform itemViewContainer,
            IGameDataProvider gameDataProvider)
        {
            _view = view;
            _popupButtonView = popupButtonView;
            _itemConfigs = itemConfigs;
            _itemPrefab = itemPrefab;
            _itemViewContainer = itemViewContainer;
            _gameDataProvider = gameDataProvider;

            _popupButtonView.OnButtonClicked += PopupButtonViewOnButtonClicked;
            _view.OnCloseButtonClicked += OnCloseButtonClicked;
            
            InitItems();
        }

        private void InitItems()
        {
            foreach (var config in _itemConfigs)
            {
                var view = Object.Instantiate(_itemPrefab, _itemViewContainer);
                new UpgradeShopItemViewPresenter(view, config, _gameDataProvider);
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