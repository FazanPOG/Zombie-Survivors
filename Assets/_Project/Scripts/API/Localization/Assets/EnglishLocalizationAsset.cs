using System;
using System.Collections.Generic;

namespace _Project.API
{
    public class EnglishLocalizationAsset : ILocalizationAsset
    {
        private Dictionary<string, string> _translations;
        
        public EnglishLocalizationAsset()
        {
            _translations = new Dictionary<string, string>()
            {
                [LocalizationKeys.GAME_NAME_KEY] = "Zombie Survivors 3D",
                [LocalizationKeys.SHOP_KEY] = "Shop",
                [LocalizationKeys.PLAY_KEY] = "Play",
                [LocalizationKeys.SETTINGS_KEY] = "Settings",
                [LocalizationKeys.MUSIC_KEY] = "Music",
                [LocalizationKeys.SOUND_KEY] = "Sound",
                [LocalizationKeys.BULLET_KEY] = "Bullet",
                [LocalizationKeys.UPGRADE_KEY] = "Upgrade",
                [LocalizationKeys.HP_KEY] = "Health",
                [LocalizationKeys.MOVE_SPEED_KEY] = "Move speed",
                [LocalizationKeys.LEVEL_SHORT_KEY] = "LV",
                [LocalizationKeys.TAP_TO_START_KEY] = "Tap to start",
                [LocalizationKeys.SCORE_KEY] = "Score",
                [LocalizationKeys.PAUSE_KEY] = "Pause",
                [LocalizationKeys.CONTINUE_KEY] = "Continue",
                [LocalizationKeys.GIVE_UP_KEY] = "Give up",
                [LocalizationKeys.MAX_KEY] = "Max",
                [LocalizationKeys.SOLD_OUT_KEY] = "Sold out",
                [LocalizationKeys.LOSE_TITLE_KEY] = "YOU'VE BEEN INFECTED",
            };
        }
        
        public string GetTranslation(string key)
        {
            if(_translations.TryGetValue(key, out string value) == false)
                throw new Exception($"Missing localization key: {key} on localization asset: {nameof(EnglishLocalizationAsset)}");

            return value;
        }
    }
}