using _Project.UI;
using UnityEngine;
using Zenject;

namespace _Project.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUI _gameplayUIPrefab;
        [SerializeField] private Environment _testEnvironmentPrefab; //TODO: change to pick in main menu
        [SerializeField] private CameraSystem _cameraSystemPrefab;
        
        public override void InstallBindings()
        {
            BindGameplayUI();
            BindEnvironment();
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

        private void BindPlayer()
        {
            var cameraSystem = Container.InstantiatePrefabForComponent<CameraSystem>(_cameraSystemPrefab);
            Container.Bind<CameraSystem>().FromInstance(cameraSystem).AsSingle().NonLazy();
        }
    }
}
