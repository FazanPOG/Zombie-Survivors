using System;
using System.Linq;
using _Project.MainMenu;
using ObservableCollections;
using R3;

namespace _Project.Data
{
    public class GameDataProxy
    {
        public readonly ReactiveProperty<int> BestScore = new ReactiveProperty<int>();
        public readonly ReactiveProperty<float> MusicVolume = new ReactiveProperty<float>();
        public readonly ReactiveProperty<float> SoundVolume = new ReactiveProperty<float>();
        public readonly ReactiveProperty<GameplayEnterParams> GameplayEnterParams = new ReactiveProperty<GameplayEnterParams>();
        public readonly ReactiveProperty<int> SoftCurrency = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> HardCurrency = new ReactiveProperty<int>();
        public readonly ReactiveProperty<int> PlayerHealth = new ReactiveProperty<int>();
        public readonly ReactiveProperty<float> PlayerMoveSpeed = new ReactiveProperty<float>();
        public readonly ObservableList<UpgradeShopItemData> UpgradeItemDatas = new ObservableList<UpgradeShopItemData>();
        public readonly ObservableList<BulletShopItemData> BulletItemDatas = new ObservableList<BulletShopItemData>();

        public GameDataProxy(GameData data)
        {
            BestScore.Value = data.BestScore;
            MusicVolume.Value = data.MusicVolume;
            SoundVolume.Value = data.SoundVolume;
            GameplayEnterParams.Value = data.GameplayEnterParams;
            SoftCurrency.Value = data.SoftCurrency;
            HardCurrency.Value = data.HardCurrency;
            PlayerHealth.Value = data.PlayerHealth;
            PlayerMoveSpeed.Value = data.PlayerMoveSpeed;

            foreach (var shopItemData in data.UpgradeItemDatas)
                UpgradeItemDatas.Add(shopItemData);
            
            foreach (var bulletItemData in data.BulletItemDatas)
                BulletItemDatas.Add(bulletItemData);
            
            BestScore.Subscribe(value => data.BestScore = value);
            MusicVolume.Subscribe(value => data.MusicVolume = value);
            SoundVolume.Subscribe(value => data.SoundVolume = value);
            GameplayEnterParams.Subscribe(value => data.GameplayEnterParams = value);
            SoftCurrency.Subscribe(value => data.SoftCurrency = value);
            HardCurrency.Subscribe(value => data.HardCurrency = value);
            PlayerHealth.Subscribe(value => data.PlayerHealth = value);
            PlayerMoveSpeed.Subscribe(value => data.PlayerMoveSpeed = value);

            UpgradeItemDatas.ObserveAdd().Subscribe((shopItemData) =>
            {
                var newData = shopItemData.Value;
                
                if(data.UpgradeItemDatas.Any(x => x.ID == newData.ID))
                    throw new Exception();
                
                data.UpgradeItemDatas.Add(shopItemData.Value);
            });
            
            UpgradeItemDatas.ObserveRemove().Subscribe((shopItemData) =>
            {
                var removedData = shopItemData.Value;
                
                if(data.UpgradeItemDatas.Any(x => x.ID == removedData.ID) == false)
                    throw new Exception();
                
                data.UpgradeItemDatas.Remove(shopItemData.Value);
            });
            
            BulletItemDatas.ObserveAdd().Subscribe((bulletItemData) =>
            {
                var newData = bulletItemData.Value;
                
                if(data.BulletItemDatas.Any(x => x.ID == newData.ID))
                    throw new Exception();
                
                data.BulletItemDatas.Add(bulletItemData.Value);
            });
            
            BulletItemDatas.ObserveRemove().Subscribe((bulletItemData) =>
            {
                var removedData = bulletItemData.Value;
                
                if(data.BulletItemDatas.Any(x => x.ID == removedData.ID) == false)
                    throw new Exception();
                
                data.BulletItemDatas.Remove(bulletItemData.Value);
            });
        }
    }
}