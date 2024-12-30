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
        [SerializeField] private BaseBoost[] _boostPrefabs;
        [SerializeField] private WeaponBoost _weaponBoostPrefab;
        [SerializeField] private WeaponConfig[] _weaponBoostConfigs;
        [SerializeField] private WeaponConfig _testWeapon;

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
            BindSpawnerServices();
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
            LevelScore levelScore = new LevelScore(new LevelScoreData(0, 100));
            
            Container.Bind<PlayerHealth>().FromInstance(playerHealth).AsSingle().NonLazy();
            Container.Bind<PlayerMoveSpeed>().FromInstance(moveSpeed).AsSingle().NonLazy();
            Container.Bind<LevelScore>().FromInstance(levelScore).AsSingle().NonLazy();
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
            
            _playerInstance.Init(input, playerHealth, playerMoveSpeed, _testWeapon, audioPlayer);
            _cameraSystemInstance.Init(_playerInstance.transform);
        }

        private void BindFactories()
        {
            var levelProgressService = Container.Resolve<IScoreService>();
            var pauseService = Container.Resolve<IPauseService>();
            var zombieCounter = Container.Resolve<IZombieCounterService>();
            var boostCounter = Container.Resolve<IBoostCounterService>();
            var audioPlayer = Container.Resolve<AudioPlayer>();
            
            ZombieFactory zombieFactory = new ZombieFactory(
                _zombiePrefab,
                _zombieConfigs,
                levelProgressService,
                pauseService,
                zombieCounter);

            BoostFactory boostFactory = new BoostFactory(
                Container,
                _boostPrefabs,
                _weaponBoostPrefab,
                _weaponBoostConfigs,
                audioPlayer,
                boostCounter
                );
            
            Container.Bind<ZombieFactory>().FromInstance(zombieFactory).AsSingle().NonLazy();
            Container.Bind<BoostFactory>().FromInstance(boostFactory).AsSingle().NonLazy();
        }

        private void BindSpawnerServices()
        {
            var context = Container.Resolve<MonoBehaviourContext>();
            var levelScore = Container.Resolve<LevelScore>();
            var zombieFactory = Container.Resolve<ZombieFactory>();
            var boostFactory = Container.Resolve<BoostFactory>();
            var environment = Container.Resolve<Environment>();
            var zombieCounter = Container.Resolve<IZombieCounterService>();
            var boostCounterService = Container.Resolve<IBoostCounterService>();

            ZombieSpawnerService zombieSpawnerService = new ZombieSpawnerService(
                context,
                levelScore,
                zombieFactory,
                environment.ZombieSpawnPoints,
                _playerInstance.transform,
                zombieCounter
            );
            
            BoostSpawningService boostSpawningService = new BoostSpawningService(
                context,
                environment.BoostSpawnPoints,
                _playerInstance.transform,
                boostFactory,
                boostCounterService,
                levelScore
                );
            
            Container.Bind<IZombieSpawnerService>().To<ZombieSpawnerService>().FromInstance(zombieSpawnerService).AsSingle().NonLazy();
            Container.Bind<IBoostSpawnerService>().To<BoostSpawningService>().FromInstance(boostSpawningService).AsSingle().NonLazy();
        }
        
        private void BindServices()
        {
            Container.Bind<IScoreService>().To<ScoreService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IPauseService>().To<PauseService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IZombieCounterService>().To<ZombieCounterService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IBoostCounterService>().To<BoostCounterService>().FromNew().AsSingle().NonLazy();
        }

        private void BindGameStateMachine()
        {
            var player = Container.Resolve<Player>();
            var playerHealth = Container.Resolve<PlayerHealth>();
            var levelProgress = Container.Resolve<LevelScore>();
            var zombieSpawnerService = Container.Resolve<IZombieSpawnerService>();
            var boostSpawnerService = Container.Resolve<IBoostSpawnerService>();
            var pauseService = Container.Resolve<IPauseService>();
            var environment = Container.Resolve<Environment>();

            _gameStateMachine = new GameStateMachine(player, playerHealth, levelProgress, zombieSpawnerService, boostSpawnerService, pauseService, environment);

            Container.Bind<IGameStateMachine>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
            Container.Bind<IGameStateProvider>().To<GameStateMachine>().FromInstance(_gameStateMachine).AsCached().NonLazy();
        }
    }
}
