using System;
using System.Collections.Generic;
using _Project.MainMenu;

namespace _Project.Data
{
    [Serializable]
    public class GameData
    {
        public float MusicVolume;
        public float SoundVolume;
        public GameplayEnterParams GameplayEnterParams;
        public int PlayerHealth;
        public float PlayerMoveSpeed;
        public int SoftCurrency;
        public int HardCurrency;
        public List<UpgradeShopItemData> UpgradeItemDatas;
        public List<BulletShopItemData> BulletItemDatas;
    }
}