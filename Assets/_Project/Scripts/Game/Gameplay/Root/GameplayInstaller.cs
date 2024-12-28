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
        
        public override void InstallBindings()
        {
            BindGameplayUI();
            BindEnvironment();
            BindInput();
            BindPlayer();
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
            var spawnPoints = Container.Resolve<Environment>().SpawnPoints;
            var randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
            
            var cameraSystem = Container.InstantiatePrefabForComponent<CameraSystem>(_cameraSystemPrefab);
            Container.Bind<CameraSystem>().FromInstance(cameraSystem).AsSingle().NonLazy();

            var playerInstance = Container.InstantiatePrefabForComponent<Player>(_playerPrefab, randomSpawnPoint.position, quaternion.identity, null);
            
            playerInstance.Init(input);
            cameraSystem.Init(playerInstance.transform);
        }
    }
}
