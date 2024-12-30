using _Project.Audio;
using _Project.Data;
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
            
            var gameDataProvider = new PlayerPrefsGameDataProvider(_defaultData);
            gameDataProvider.LoadGameData();
            
            Container.Bind<IGameDataProvider>().To<PlayerPrefsGameDataProvider>().FromInstance(gameDataProvider).AsSingle().NonLazy();
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
            var gameDataProvider = Container.Resolve<IGameDataProvider>();
            
            var soundAudioSource = new GameObject("[Sound]").AddComponent<AudioSource>();
            var backgroundMusicAudioSource = new GameObject("[Music]").AddComponent<AudioSource>();
            
            DontDestroyOnLoad(soundAudioSource.gameObject);
            DontDestroyOnLoad(backgroundMusicAudioSource.gameObject);
            
            AudioPlayer soundAudioPlayer = new AudioPlayer(soundAudioSource, gameDataProvider);
            BackgroundMusic backgroundMusic = new BackgroundMusic(backgroundMusicAudioSource, gameDataProvider);
            
            if(_defaultData.BackgroundMusic != null)
                backgroundMusic.PlayMusic(_defaultData.BackgroundMusic);
            
            Container.Bind<AudioPlayer>().FromInstance(soundAudioPlayer).AsSingle().NonLazy();
            Container.Bind<BackgroundMusic>().FromInstance(backgroundMusic).AsSingle().NonLazy();
        }

        private void StartGame()
        {
            var sceneLoader = Container.Resolve<ISceneLoaderService>();

            var currentScene = SceneManager.GetActiveScene().name;
            
            switch (currentScene)
            {
                case Scenes.Boot:
                    sceneLoader.LoadSceneAsync(Scenes.MainMenu);
                    break;
                
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
