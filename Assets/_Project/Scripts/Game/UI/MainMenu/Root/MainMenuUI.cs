using _Project.Data;
using _Project.Root;
using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private PlayButtonView _playButtonView;
        [SerializeField] private PopupButtonView _settingsButtonView;
        [Header("Popups")]
        [SerializeField] private SettingsPopupView _settingsPopupView;
        
        private DiContainer _container;
        
        public void Bind(DiContainer diContainer)
        {
            _container = diContainer;

            BindPresenters();
        }

        private void BindPresenters()
        {
            var sceneLoader = _container.Resolve<ISceneLoaderService>();
            var gameDataProvider = _container.Resolve<IGameDataProvider>();
            
            new PlayButtonViewPresenter(_playButtonView, sceneLoader);
            new SettingsPopupViewPresenter(_settingsButtonView, _settingsPopupView, gameDataProvider);
        }
    }
}