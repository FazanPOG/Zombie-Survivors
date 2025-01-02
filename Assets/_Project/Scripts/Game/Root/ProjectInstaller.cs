using _Project.API;
using _Project.Audio;
using _Project.Data;
using _Project.Scripts.Game.Data;
using _Project.UI;
using _Project.Utility;
using UnityEngine;
using Zenject;

namespace _Project.Root
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRootPrefab;
        [SerializeField] private DefaultDataConfig _defaultData;
        [SerializeField] private CommonAudioClipsConfig _commonAudio;
        
        public override void InstallBindings()
        {
            BindData();
            BindAPI();
            BindUtility();
            BindUIRoot();
            BindServices();
            BindAudio();

            StartGame();
        }

        private void BindAPI()
        {
            var apiBinder = new APIBinder(Container);
            apiBinder.Bind();
        }

        private void BindData()
        {
            var gameDataProvider = new PlayerPrefsGameDataProvider(_defaultData);
            gameDataProvider.LoadGameData();
            
            Container.Bind<IGameDataProvider>().To<PlayerPrefsGameDataProvider>().FromInstance(gameDataProvider).AsSingle().NonLazy();
            Container.Bind<CommonAudioClipsConfig>().FromInstance(_commonAudio).AsSingle().NonLazy();
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
            var commonAudioClipsConfig = Container.Resolve<CommonAudioClipsConfig>();
            
            var soundAudioSource = new GameObject("[Sound]").AddComponent<AudioSource>();
            var backgroundMusicAudioSource = new GameObject("[Music]").AddComponent<AudioSource>();
            
            DontDestroyOnLoad(soundAudioSource.gameObject);
            DontDestroyOnLoad(backgroundMusicAudioSource.gameObject);
            
            AudioPlayer soundAudioPlayer = new AudioPlayer(soundAudioSource, gameDataProvider, commonAudioClipsConfig);
            BackgroundMusic backgroundMusic = new BackgroundMusic(backgroundMusicAudioSource, gameDataProvider);
            
            if(_defaultData.BackgroundMusic != null)
                backgroundMusic.PlayMusic(_defaultData.BackgroundMusic);
            
            Container.Bind<AudioPlayer>().FromInstance(soundAudioPlayer).AsSingle().NonLazy();
            Container.Bind<BackgroundMusic>().FromInstance(backgroundMusic).AsSingle().NonLazy();
        }

        private void StartGame()
        {
            var context = Container.Resolve<MonoBehaviourContext>();
            var sceneLoaderService = Container.Resolve<ISceneLoaderService>();
            var apiEnvironmentService = Container.Resolve<IAPIEnvironmentService>();
            var localizationProvider = Container.Resolve<ILocalizationProvider>();
            var uiRoot = Container.Resolve<UIRoot>();
            
            new Boot(context, sceneLoaderService, apiEnvironmentService, localizationProvider, uiRoot);
        }
    }
}
