using System;
using YG;

namespace _Project.API
{
    public class YandexGamesDeviceProvider : IDeviceProvider
    {
        private const string DESKTOP = "desktop";
        private const string MOBILE = "mobile";
        private const string TABLET = "tablet";
        
        public DeviceType GetCurrentDevice()
        {
            string device = YandexGame.EnvironmentData.deviceType;
            
            if(string.IsNullOrEmpty(device))
                throw new Exception($"Something gone wrong: {nameof(YandexGamesDeviceProvider)}");
            
            switch (YandexGame.EnvironmentData.deviceType)
            {
                case DESKTOP:
                    return DeviceType.Desktop; 
                
                case MOBILE:
                    return DeviceType.Mobile;
                    
                case TABLET:
                    return DeviceType.Mobile;
            }

            throw new Exception($"Device does not support: {YandexGame.EnvironmentData.language}");
        }
    }
}