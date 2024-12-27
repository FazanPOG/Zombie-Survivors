using _Project.UI;
using UnityEngine;
using Zenject;

namespace _Project.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuUI _mainMenuUIPrefab;
        
        public override void InstallBindings()
        {
            BindMainMenuUI();
        }
        
        private void BindMainMenuUI()
        {
            var uiRoot = Container.Resolve<UIRoot>();
            var mainMenuUI = Container.InstantiatePrefabForComponent<MainMenuUI>(_mainMenuUIPrefab);
            
            uiRoot.AttachSceneUI(mainMenuUI.gameObject);
            mainMenuUI.Bind(Container);
        }
    }
}