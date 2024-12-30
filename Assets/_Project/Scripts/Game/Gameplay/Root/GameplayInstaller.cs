using _Project.Audio;
using _Project.Data;
using _Project.Scripts.Game.Data;
using _Project.UI;
using _Project.Utility;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUI _gameplayUIPrefab;
        [SerializeField] private CameraSystem _cameraSystemPrefab;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Zombie _zombiePrefab;
        [SerializeField] private ZombieConfig[] _zombieConfigs;

        private Player _playerInstance;
        private CameraSystem _cameraSystemInstance;
        private GameStateMachine _gameStateMachine;
        
        public override void InstallBindings()
        {
            BindData();
            BindEnvironment();
            BindInput();
            InstantiatePlayer();
            BindServices();
            BindFactories();
            BindZombieSpawnerService();
            InitPlayer();
            BindGameStateMachine();
            BindGameplayUI();

            Container.Resolve<IGameStateMachine>().EnterIn<BootState>();
        }

        private void BindData()
        {
            var config = Container.Resolve<DefaultDataConfig>();
            PlayerHealth playerHealth = new PlayerHealth(new PlayerHealthData(config.Health));
            PlayerMoveSpeed moveSpeed = new PlayerMoveSpeed(new PlayerMoveSpeedData(config.PlayerMoveSpeed));
            LevelProgress levelProgress = new LevelProgress(new LevelProgressData(0, 100));
            
            Container.Bind<PlayerHealth>().FromInstance(playerHealth).AsSingle().NonLazy();
            Container.Bind<PlayerMoveSpeed>().FromInstance(moveSpeed).AsSingle().NonLazy();
            Container.Bind<LevelProgress>().FromInstance(levelProgress).AsSingle().NonLazy();
        }

        private void BindGameplayUI()
        {
            var uiRoot = Container.Resolve<UIRoot>();
            var gameplayUI = Container.InstantiatePrefabForComponent<GameplayUI>(_gameplayUIPrefab);
            
            uiRoot.AttachSceneUI(gameplayUI.gameObject);
            gameplayUI.Bind(Container);
        }

        private void BindEnvironment()
        {
            var config = Container.Resolve<DefaultDataConfig>();
            
            var environment = Container.InstantiatePrefabForComponent<Environment>(config.Environment);
            Container.Bind<Environment>().FromInstance(environment).AsSingle().NonLazy();
        }

        private void BindInput()
        {
            Container.BindInterfacesTo<Input>().FromNew().AsSingle().NonLazy();
        }

        private void InstantiatePlayer()
        {
            var spawnPoints = Container.Resolve<Environment>().PlayerSpawnPoints;
            var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            
            _playerInstance = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, randomSpawnPoint.position, quaternion.identity, null);
            Container.Bind<Player>().FromInstance(_playerInstance).AsSingle().NonLazy();
            
            _cameraSystemInstance = Container.InstantiatePrefabForComponent<CameraSystem>(_cameraSystemPrefab);
            Container.Bind<CameraSystem>().FromInstance(_cameraSystemInstance).AsSingle().NonLazy();
        }
        
        private void InitPlayer()
        {
            var input = Container.Resolve<IInput>();
            var playerHealth = Container.Resolve<PlayerHealth>();
            var playerMoveSpeed = Container.Resolve<PlayerMoveSpeed>();
            var audioPlayer = Container.Resolve<AudioPlayer>();
            
            _playerInstance.Init(input, playerHealth, playerMoveSpeed, audioPlayer);
            _cameraSystemInstance.Init(_playerInstance.transform);
        }

        private void BindFactories()
        {
            var levelProgressService = Container.Resolve<ILevelProgressService>();
            var pauseService = Container.Resolve<IPauseService>();
            
            ZombieFactory zombieFactory = new ZombieFactory(
                _zombiePrefab,
                _zombieConfigs,
                levelProgressService,
                pauseService);

            Container.Bind<ZombieFactory>().FromInstance(zombieFactory).AsSingle().NonLazy();
        }

        private void BindZombieSpawnerService()
        {
            var context = Container.Resolve<MonoBehaviourContext>();
            var levelProgress = Container.Resolve<LevelProgress>();
            var zombieFactory = Container.Resolve<ZombieFactory>();
            var environment = Container.Resolve<Environment>();
            
            ZombieSpawnerService zombieSpawnerService = new ZombieSpawnerService(
                context,
                levelProgress,
                zombieFactory,
                environment.ZombieSpawnPoints,
                _playerInstance.transform
            );
            
            Container.Bind<IZombieSpawnerService>().To<ZombieSpawnerService>().FromInstance(zombieSpawnerService).AsSingle().NonLazy();
        }
        
        private void BindServices()
        {
            Container.Bind<ILevelProgressService>().To<LevelProgressService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IPauseService>().To<PauseService>().FromNew().AsSingle().NonLazy();
        }

        private void BindGameStateMachine()
        {
            var player = Container.Resolve<Player>();
            var playerHealth = Container.Resolve<PlayerHealth>();
            var levelProgress = Container.Resolve<LevelProgress>();
            var zombieSpawnerService = Container.Resolve<IZombieSpawnerService>();
            var pauseService = Container.Resolve<IPauseService>();
            
            _gameStateMachine = new GameStateMachine(player, playerHealth, levelProgress, zombieSpawnerService, pauseService);

            Container.Bind<IGameStateMachine>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
            Container.Bind<IGameStateProvider>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
        }
    }
}
