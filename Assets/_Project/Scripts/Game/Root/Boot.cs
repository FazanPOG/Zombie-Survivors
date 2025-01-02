using System.Collections;
using _Project.API;
using _Project.UI;
using _Project.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Root
{
    public class Boot
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IAPIEnvironmentService _apiEnvironmentService;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly UIRoot _uiRoot;

        public Boot(
            MonoBehaviourContext context, 
            ISceneLoaderService sceneLoaderService, 
            IAPIEnvironmentService apiEnvironmentService,
            ILocalizationProvider localizationProvider,
            UIRoot uiRoot)
        {
            _sceneLoaderService = sceneLoaderService;
            _apiEnvironmentService = apiEnvironmentService;
            _localizationProvider = localizationProvider;
            _uiRoot = uiRoot;

            context.StartCoroutine(WaitAPILoad());
        }

        private IEnumerator WaitAPILoad()
        {
            yield return new WaitUntil(() => _apiEnvironmentService.IsReady);
            _localizationProvider.LoadLocalizationAsset();
            _uiRoot.Init(_localizationProvider);
            StartGame();
        }

        private void StartGame()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            
            switch (currentScene)
            {
                case Scenes.Boot:
                    _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu);
                    break;
                
                case Scenes.MainMenu:
                    _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu);
                    break;
                
                case Scenes.Gameplay:
                    _sceneLoaderService.LoadSceneAsync(Scenes.Gameplay);
                    break;
            }
        }
    }
}