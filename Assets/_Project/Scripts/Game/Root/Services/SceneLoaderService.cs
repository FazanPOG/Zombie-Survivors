using System.Collections;
using _Project.UI;
using _Project.Utility;
using UnityEngine.SceneManagement;

namespace _Project.Root
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly MonoBehaviourContext _monoBehaviourContext;
        private readonly UIRoot _uiRoot;

        public SceneLoaderService(MonoBehaviourContext monoBehaviourContext, UIRoot uiRoot)
        {
            _monoBehaviourContext = monoBehaviourContext;
            _uiRoot = uiRoot;
        }

        public void LoadSceneAsync(string sceneName)
        {
            _monoBehaviourContext.StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            _uiRoot.ShowLoadingScreen();
            yield return SceneManager.LoadSceneAsync(Scenes.Boot);
            yield return SceneManager.LoadSceneAsync(sceneName);
            _uiRoot.HideLoadingScreen();
        }
    }
}