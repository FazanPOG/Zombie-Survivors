using System;

namespace _Project.API
{
    public interface IADService
    {
        bool IsFullscreenAvailable { get; }
        bool IsRewardedAvailable { get; }

        event Action<bool> OnFullscreenClose;
        event Action<string> OnRewardedReward;
        
        void ShowFullscreen();
        void ShowRewarded(string rewardID);
    }
}