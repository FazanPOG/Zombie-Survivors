using System;
using YG;

namespace _Project.API
{
    public class YandexGamesADService : IADService
    {
        private bool _isFullscreenAvailable;

        public bool IsFullscreenAvailable => true;
        public bool IsRewardedAvailable => true;
        
        public event Action<bool> OnFullscreenClose;
        public event Action<string> OnRewardedReward;

        private string _currentRewardID;

        public YandexGamesADService()
        {
            _isFullscreenAvailable = false;
            
            YandexGame.RewardVideoEvent += RewardVideoEvent;
            YandexGame.CloseFullAdEvent += OnFullscreenCloseInvoke;
        }

        private void RewardVideoEvent(int _)
        {
            if(string.IsNullOrEmpty(_currentRewardID))
                throw new NullReferenceException($"Missing AD reward");
            
            OnRewardedReward?.Invoke(_currentRewardID);
            _currentRewardID = String.Empty;
        }

        private void OnFullscreenCloseInvoke() => OnFullscreenClose?.Invoke(true);

        public void ShowFullscreen()
        {
            YandexGame.FullscreenShow();
            _isFullscreenAvailable = false;
        }

        public void ShowRewarded(string rewardID)
        {
            _currentRewardID = rewardID;
            YandexGame.RewVideoShow(0);
        }
    }
}