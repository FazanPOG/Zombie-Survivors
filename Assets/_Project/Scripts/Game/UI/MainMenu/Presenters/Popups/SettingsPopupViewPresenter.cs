using _Project.Data;

namespace _Project.UI
{
    public class SettingsPopupViewPresenter
    {
        private readonly SettingsPopupView _settingsPopupView;
        private readonly IGameDataProvider _gameDataProvider;

        public SettingsPopupViewPresenter(PopupButtonView settingsButtonView, SettingsPopupView settingsPopupView, IGameDataProvider gameDataProvider)
        {
            _settingsPopupView = settingsPopupView;
            _gameDataProvider = gameDataProvider;

            settingsButtonView.OnButtonClicked += ShowPopup;
            _settingsPopupView.OnCloseButtonClicked += HidePopup;
            _settingsPopupView.OnMusicSliderChanged += ChangeMusicVolume;
            _settingsPopupView.OnSoundSliderChanged += ChangeSoundVolume;
            
            _settingsPopupView.SetMusicSliderValue(_gameDataProvider.GameDataProxy.MusicVolume.CurrentValue);
            _settingsPopupView.SetSoundSliderValue(_gameDataProvider.GameDataProxy.SoundVolume.CurrentValue);
            HidePopup();
        }

        private void ChangeMusicVolume(float value)
        {
            _gameDataProvider.GameDataProxy.MusicVolume.Value = value;
            _gameDataProvider.SaveGameData();
        }

        private void ChangeSoundVolume(float value)
        {
            _gameDataProvider.GameDataProxy.SoundVolume.Value = value;
            _gameDataProvider.SaveGameData();
        }

        private void ShowPopup()
        {
            _settingsPopupView.Show();
        }

        private void HidePopup()
        {
            _settingsPopupView.Hide();
        }
    }
}