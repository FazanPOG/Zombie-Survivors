using _Project.Gameplay;
using _Project.UI;
using _Project.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Input = _Project.Gameplay.Input;

namespace _Project.Root
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRootPrefab;
        
        public override void InstallBindings()
        {
            BindUtility();
            BindUIRoot();
            BindServices();
            
            StartGame();
        }

        private void BindUtility()
        {
            var monoBehaviourContext = new GameObject("[MonoBehaviourContext]").AddComponent<MonoBehaviourContext>();
            DontDestroyOnLoad(monoBehaviourContext.gameObject);
            Container.Bind<MonoBehaviourContext>().FromInstance(monoBehaviourContext).AsSingle().NonLazy();
        }

        private void BindUIRoot()
        {
            var uiRoot = Container.InstantiatePrefabForComponent<UIRoot>(_uiRootPrefab);
            Container.Bind<UIRoot>().FromInstance(uiRoot).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().FromNew().AsSingle().NonLazy();
        }
        
        private void StartGame()
        {
            var sceneLoader = Container.Resolve<ISceneLoaderService>();

            var currentScene = SceneManager.GetActiveScene().name;
            
            switch (currentScene)
            {
                case Scenes.MainMenu:
                    sceneLoader.LoadSceneAsync(Scenes.MainMenu);
                    break;
                
                case Scenes.Gameplay:
                    sceneLoader.LoadSceneAsync(Scenes.Gameplay);
                    break;
            }
        }
    }
}
