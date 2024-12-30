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
            }
            
            return GameDataProxy;
        }

        public void SaveGameData()
        {
            var json = JsonUtility.ToJson(_originData, true);
            PlayerPrefs.SetString(GAME_DATA_KEY, json);
            PlayerPrefs.Save();
        }

        private GameDataProxy CreateGameDataFromSettings()
        {
            _originData = new GameData()
            {
                MusicVolume = _defaultDataConfig.MusicVolume,
                SoundVolume = _defaultDataConfig.SoundVolume
            };
            
            return new GameDataProxy(_originData);
        }
    }
}