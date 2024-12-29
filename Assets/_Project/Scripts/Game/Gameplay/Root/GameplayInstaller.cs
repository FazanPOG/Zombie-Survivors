using _Project.Data;
using _Project.Game;
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
        [SerializeField] private Environment _testEnvironmentPrefab; //TODO: change to pick in main menu
        [SerializeField] private CameraSystem _cameraSystemPrefab;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Zombie _zombiePrefab;
        [SerializeField] private ZombieConfig[] _zombieConfigs;

        private GameStateMachine _gameStateMachine;
        
        public override void InstallBindings()
        {
            BindData();
            BindEnvironment();
            BindInput();
            BindPlayer();
            BindFactories();
            BindServices();
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
            Container.Bind<ILevelProgressService>().To<LevelProgressService>().FromNew().AsSingle().NonLazy();
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
            var environment = Container.InstantiatePrefabForComponent<Environment>(_testEnvironmentPrefab);
            Container.Bind<Environment>().FromInstance(environment).AsSingle().NonLazy();
        }

        private void BindInput()
        {
            Container.BindInterfacesTo<Input>().FromNew().AsSingle().NonLazy();
        }

        private void BindPlayer()
        {
            var input = Container.Resolve<IInput>();
            var playerHealth = Container.Resolve<PlayerHealth>();
            var playerMoveSpeed = Container.Resolve<PlayerMoveSpeed>();
            var spawnPoints = Container.Resolve<Environment>().PlayerSpawnPoints;
            var audioPlayer = Container.Resolve<AudioPlayer>();
            
            var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            
            var cameraSystem = Container.InstantiatePrefabForComponent<CameraSystem>(_cameraSystemPrefab);
            Container.Bind<CameraSystem>().FromInstance(cameraSystem).AsSingle().NonLazy();

            var playerInstance = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, randomSpawnPoint.position, quaternion.identity, null);
            Container.Bind<Player>().FromInstance(playerInstance).AsSingle().NonLazy();
            
            playerInstance.Init(input, playerHealth, playerMoveSpeed, audioPlayer);
            cameraSystem.Init(playerInstance.transform);
        }

        private void BindFactories()
        {
            var levelProgressService = Container.Resolve<ILevelProgressService>();
            
            ZombieFactory zombieFactory = new ZombieFactory(
                _zombiePrefab,
                _zombieConfigs,
                levelProgressService);

            Container.Bind<ZombieFactory>().FromInstance(zombieFactory).AsSingle().NonLazy();
        }

        private void BindServices()
        {
            var context = Container.Resolve<MonoBehaviourContext>();
            var levelProgress = Container.Resolve<LevelProgress>();
            var zombieFactory = Container.Resolve<ZombieFactory>();
            var player = Container.Resolve<Player>();
            
            ZombieSpawnerService zombieSpawnerService = new ZombieSpawnerService(
                context,
                levelProgress,
                zombieFactory,
                _testEnvironmentPrefab.ZombieSpawnPoints,
                player.transform
                );
            
            Container.Bind<IZombieSpawnerService>().To<ZombieSpawnerService>().FromInstance(zombieSpawnerService).AsSingle().NonLazy();
        }

        private void BindGameStateMachine()
        {
            var player = Container.Resolve<Player>();
            var playerHealth = Container.Resolve<PlayerHealth>();
            var levelProgress = Container.Resolve<LevelProgress>();
            var zombieSpawnerService = Container.Resolve<IZombieSpawnerService>();
            
            _gameStateMachine = new GameStateMachine(player, playerHealth, levelProgress, zombieSpawnerService);

            Container.Bind<IGameStateMachine>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
            Container.Bind<IGameStateProvider>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
        }
    }
}
