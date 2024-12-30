using System;
using _Project.Gameplay;
using _Project.Root;
using _Project.Utility;

namespace _Project.UI
{
    public class PausePopupViewPresenter : IDisposable
    {
        private readonly PausePopupView _pausePopupView;
        private readonly PopupButtonView _buttonView;
        private readonly IPauseService _pauseService;
        private readonly ISceneLoaderService _sceneLoaderService;

        public PausePopupViewPresenter(
            PausePopupView pausePopupView, 
            PopupButtonView buttonView, 
            IPauseService pauseService, 
            ISceneLoaderService sceneLoaderService)
        {
            _pausePopupView = pausePopupView;
            _buttonView = buttonView;
            _pauseService = pauseService;
            _sceneLoaderService = sceneLoaderService;

            _buttonView.OnButtonClicked += Pause;
            _pausePopupView.OnCloseButtonClicked += Resume;
            _pausePopupView.OnResumeButtonClicked += Resume;
            _pausePopupView.OnExitButtonClicked += Exit;
            
            _pausePopupView.Hide();
        }

        private void Pause()
        {
            _pausePopupView.Show();
            _pauseService.SetPause(true);
        }

        private void Resume()
        {
            _pausePopupView.Hide();
            _pauseService.SetPause(false);
        }

        private void Exit()
        {
            _sceneLoaderService.LoadSceneAsync(Scenes.MainMenu);
        }
        
        public void Dispose()
        {
            _buttonView.OnButtonClicked -= Pause;
            _pausePopupView.OnCloseButtonClicked -= Resume;
            _pausePopupView.OnResumeButtonClicked -= Resume;
            _pausePopupView.OnExitButtonClicked -= Exit;
        }
    }
}