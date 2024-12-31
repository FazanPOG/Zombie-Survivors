using System;
using System.Collections.Generic;
using _Project.MainMenu;
using _Project.Scripts.Game.Data;
using UnityEngine;

namespace _Project.Data
{
    public class PlayerPrefsGameDataProvider : IGameDataProvider
    {
        private const string GAME_DATA_KEY = nameof(GAME_DATA_KEY);
        
        private readonly DefaultDataConfig _defaultDataConfig;

        private GameData _originData;
        
        public GameDataProxy GameDataProxy { get; private set; }

        public PlayerPrefsGameDataProvider(DefaultDataConfig defaultDataConfig)
        {
            _defaultDataConfig = defaultDataConfig;
        }
        
        public GameDataProxy LoadGameData()
        {
            if (PlayerPrefs.HasKey(GAME_DATA_KEY) == false)
            {
                GameDataProxy = CreateGameDataFromSettings();
                SaveGameData();
            }
            else
            {
                var json = PlayerPrefs.GetString(GAME_DATA_KEY);
                _originData = JsonUtility.FromJson<GameData>(json);
                GameDataProxy = new GameDataProxy(_originData);
                
                Debug.Log($"LOAD DATA: {json}");
            }
            
            return GameDataProxy;
        }

        public void SaveGameData()
        {
            var json = JsonUtility.ToJson(_originData, true);
            PlayerPrefs.SetString(GAME_DATA_KEY, json);
            PlayerPrefs.Save();
            
            Debug.Log($"SAVE DATA: {json}");
        }

        private GameDataProxy CreateGameDataFromSettings()
        {
            GameplayEnterParams defaultGameplayEnterParams = new GameplayEnterParams()
            {
                EnvironmentID = _defaultDataConfig.Environment.Config.ID,
            };
            
            _originData = new GameData()
            {
                MusicVolume = _defaultDataConfig.MusicVolume,
                SoundVolume = _defaultDataConfig.SoundVolume,
                GameplayEnterParams = defaultGameplayEnterParams,
                ShopItemDatas = new List<UpgradeShopItemData>(),
                SoftCurrency = 0,
                HardCurrency = 0,
                PlayerHealth = _defaultDataConfig.PlayerHealth,
                PlayerMoveSpeed = _defaultDataConfig.PlayerMoveSpeed,
            };
            
            return new GameDataProxy(_originData);
        }
    }
}