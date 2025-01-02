using System.Collections;
using _Project.Utility;
using UnityEngine;
using YG;

namespace _Project.API
{
    public class YandexGamesEnvironmentService : IAPIEnvironmentService
    {
        public bool IsReady { get; private set; }

        public YandexGamesEnvironmentService(MonoBehaviourContext monoBehaviourContext)
        {
            IsReady = false;
            monoBehaviourContext.StartCoroutine(WaitAPILoad());
        }

        public void GameLoadingAndReady()
        {
            YandexGame.GameReadyAPI();
        }

        public void GameplayStart()
        {
            //YandexGame.GameplayStart();
        }

        public void GameplayStop()
        {
            //YandexGame
        }

        public void SetPaused(bool isPaused)
        {
            if (isPaused)
                GameplayStart();
            else
                GameplayStop();
        }

        private IEnumerator WaitAPILoad()
        {
            yield return new WaitUntil(() => YandexGame.SDKEnabled);
            IsReady = true;
        }
    }
}