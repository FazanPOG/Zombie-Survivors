using System;

namespace _Project.UI
{
    public class PausePopupViewPresenter : IDisposable
    {
        private readonly PausePopupView _pausePopupView;

        //TODO
        public PausePopupViewPresenter(PausePopupView pausePopupView)
        {
            _pausePopupView = pausePopupView;
            
            _pausePopupView.OnCloseButtonClicked += OnCloseButtonClicked;
        }

        private void OnCloseButtonClicked()
        {
            _pausePopupView.Hide();
        }

        public void Dispose()
        {
        }
    }
}