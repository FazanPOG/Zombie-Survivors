using System;
using _Project.API;
using R3;
using YG;

namespace _Project.API
{
    public class YandexGamesLocalizationProvider : ILocalizationProvider
    {
        public ILocalizationAsset LocalizationAsset { get; private set; }
    
        public Observable<ILocalizationAsset> LoadLocalizationAsset()
        {
            ILocalizationAsset localizationAsset;
            
            string currentLanguage = YandexGame.EnvironmentData.language;

            if (String.IsNullOrEmpty(currentLanguage))
                currentLanguage = ConvertToISOFormatLanguage(Language.Russian);

            switch (currentLanguage)
            {
                case "ru":
                    localizationAsset = new RussianLocalizationAsset();
                    break;
                
                case "en":
                    localizationAsset = new EnglishLocalizationAsset();
                    break;
                
                default:
                    localizationAsset = new RussianLocalizationAsset();
                    break;
            }

            LocalizationAsset = localizationAsset;
            return Observable.Return(localizationAsset);
        }

        private string ConvertToISOFormatLanguage(Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    return "ru";
                
                case Language.English:
                    return "en";
                
                default:
                    throw new NotImplementedException($"Language does not support: {language}");
            }
        }
    }
}