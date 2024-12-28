using _Project.Data;
using _Project.Game;
using _Project.Scripts.Game.Data;
using _Project.UI;
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
        
        public override void InstallBindings()
        {
            BindPlayerData();
            BindEnvironment();
            BindInput();
            BindGameStateMachine();
            BindGameplayUI();
            BindPlayer();
            BindFactories();

            Container.Resolve<IGameStateMachine>().EnterIn<BootState>();
        }

        private void BindPlayerData()
        {
            var config = Container.Resolve<DefaultDataConfig>();
            PlayerHealth playerHealth = new PlayerHealth(new PlayerHealthData(config.Health));
            PlayerMoveSpeed moveSpeed = new PlayerMoveSpeed(new PlayerMoveSpeedData(config.PlayerMoveSpeed));

            Container.Bind<PlayerHealth>().FromInstance(playerHealth).AsSingle().NonLazy();
            Container.Bind<PlayerMoveSpeed>().FromInstance(moveSpeed).AsSingle().NonLazy();
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
            var gameStateProvider = Container.Resolve<IGameStateProvider>();
            
            var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            
            var cameraSystem = Container.InstantiatePrefabForComponent<CameraSystem>(_cameraSystemPrefab);
            Container.Bind<CameraSystem>().FromInstance(cameraSystem).AsSingle().NonLazy();

            var playerInstance = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, randomSpawnPoint.position, quaternion.identity, null);
            Container.Bind<Player>().FromInstance(playerInstance).AsSingle().NonLazy();
            
            playerInstance.Init(input, gameStateProvider, playerHealth, playerMoveSpeed, audioPlayer);
            cameraSystem.Init(playerInstance.transform);
        }

        private void BindFactories()
        {
            var player = Container.Resolve<Player>();
            
            ZombieFactory zombieFactory = new ZombieFactory(
                _zombiePrefab, 
                _testEnvironmentPrefab.ZombieSpawnPoints, 
                player);

            Container.Bind<ZombieFactory>().FromInstance(zombieFactory).AsSingle().NonLazy();
        }

        private void BindGameStateMachine()
        {
            var gameStateMachine = new GameStateMachine();
            Container.BindInterfacesTo<GameStateMachine>().FromInstance(gameStateMachine).AsSingle().NonLazy();
        }
    }
}
