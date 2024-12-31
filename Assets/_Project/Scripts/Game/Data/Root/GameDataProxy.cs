using System;
using System.Linq;
using _Project.MainMenu;
using ObservableCollections;
using R3;

namespace _Project.Data
{
    public class GameDataProxy
    {
        public readonly ReactiveProperty<float> MusicVolume = new ReactiveProperty<float>();
        public readonly ReactiveProperty<float> SoundVolume = new ReactiveProperty<float>();
        public readonly ReactiveProperty<GameplayEnterParams> GameplayEnterParams = new ReactiveProperty<GameplayEnterParams>();
        public readonly ReactiveProperty<int> SoftCurrency = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> HardCurrency = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> PlayerHealth = new ReactiveProperty<int>();
        public readonly ReactiveProperty<float> PlayerMoveSpeed = new ReactiveProperty<float>();
        public readonly ObservableList<UpgradeShopItemData> ShopItemDatas = new ObservableList<UpgradeShopItemData>();

        public GameDataProxy(GameData data)
        {
            MusicVolume.Value = data.MusicVolume;
            SoundVolume.Value = data.SoundVolume;
            GameplayEnterParams.Value = data.GameplayEnterParams;
            SoftCurrency.Value = data.SoftCurrency;
            HardCurrency.Value = data.HardCurrency;
            PlayerHealth.Value = data.PlayerHealth;
            PlayerMoveSpeed.Value = data.PlayerMoveSpeed;

            foreach (var shopItemData in data.ShopItemDatas)
                ShopItemDatas.Add(shopItemData);
            
            MusicVolume.Subscribe(value => data.MusicVolume = value);
            SoundVolume.Subscribe(value => data.SoundVolume = value);
            GameplayEnterParams.Subscribe(value => data.GameplayEnterParams = value);
            SoftCurrency.Subscribe(value => data.SoftCurrency = value);
            HardCurrency.Subscribe(value => data.HardCurrency = value);
            PlayerHealth.Subscribe(value => data.PlayerHealth = value);
            PlayerMoveSpeed.Subscribe(value => data.PlayerMoveSpeed = value);

            ShopItemDatas.ObserveAdd().Subscribe((shopItemData) =>
            {
                var newData = shopItemData.Value;
                
                if(data.ShopItemDatas.Any(x => x.ID == newData.ID))
                    throw new Exception();
                
                data.ShopItemDatas.Add(shopItemData.Value);
            });
            
            ShopItemDatas.ObserveRemove().Subscribe((shopItemData) =>
            {
                var removedData = shopItemData.Value;
                
                if(data.ShopItemDatas.Any(x => x.ID == removedData.ID) == false)
                    throw new Exception();
                
                data.ShopItemDatas.Remove(shopItemData.Value);
            });
        }
    }
}