using System;
using System.Collections.Generic;

namespace _Project.API
{
    public class RussianLocalizationAsset : ILocalizationAsset
    {
        private Dictionary<string, string> _translations;
        
        public RussianLocalizationAsset()
        {
            _translations = new Dictionary<string, string>()
            {
                [LocalizationKeys.GAME_NAME_KEY] = "ЗОМБИ ВЫЖИВАНИЕ 3д",
                [LocalizationKeys.SHOP_KEY] = "Магазин",
                [LocalizationKeys.PLAY_KEY] = "Играть",
                [LocalizationKeys.SETTINGS_KEY] = "Настройки",
                [LocalizationKeys.MUSIC_KEY] = "Музыка",
                [LocalizationKeys.SOUND_KEY] = "Звуки",
                [LocalizationKeys.BULLET_KEY] = "Патроны",
                [LocalizationKeys.UPGRADE_KEY] = "Улучшения",
                [LocalizationKeys.HP_KEY] = "Здоровье",
                [LocalizationKeys.MOVE_SPEED_KEY] = "Скорость",
                [LocalizationKeys.LEVEL_SHORT_KEY] = "УР",
                [LocalizationKeys.TAP_TO_START_KEY] = "Нажмите для начала",
                [LocalizationKeys.SCORE_KEY] = "Счет",
                [LocalizationKeys.PAUSE_KEY] = "Пауза",
                [LocalizationKeys.CONTINUE_KEY] = "Продолжить",
                [LocalizationKeys.GIVE_UP_KEY] = "Сдаться",
                [LocalizationKeys.MAX_KEY] = "МАКС",
                [LocalizationKeys.SOLD_OUT_KEY] = "Распродано",
                [LocalizationKeys.LOSE_TITLE_KEY] = "Вы были заражены!",
            };
        }
        
        public string GetTranslation(string key)
        {
            if(_translations.TryGetValue(key, out string value) == false)
                throw new Exception($"Missing localization key: {key} on localization asset: {nameof(RussianLocalizationAsset)}");

            return value;
        }
    }
}