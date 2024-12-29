using _Project.Root;
using UnityEngine;
using Zenject;

namespace _Project.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private PlayButtonView _playButtonView;
        
        private DiContainer _container;
        
        public void Bind(DiContainer diContainer)
        {
            _container = diContainer;

            BindPresenters();
        }

        private void BindPresenters()
        {
            var sceneLoader = _container.Resolve<ISceneLoaderService>();
            
            new PlayButtonViewPresenter(_playButtonView, sceneLoader);
        }
    }
}