using _Project.Game;
using _Project.Scripts.Game.Data;
using _Project.UI;
using _Project.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Root
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRootPrefab;
        [SerializeField] private DefaultDataConfig _defaultData;
        
        public override void InstallBindings()
        {
            BindData();
            BindUtility();
            BindUIRoot();
            BindServices();
            BindAudio();
            
            StartGame();
        }

        private void BindData()
        {
            Container.Bind<DefaultDataConfig>().FromInstance(_defaultData).AsSingle().NonLazy();
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

        private void BindAudio()
        {
            var audioSource = new GameObject("[Audio]").AddComponent<AudioSource>();
            DontDestroyOnLoad(audioSource.gameObject);
            AudioPlayer audioPlayer = new AudioPlayer(audioSource);
            Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle().NonLazy();
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
