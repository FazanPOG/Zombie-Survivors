using _Project.API;
using _Project.Audio;
using _Project.Data;

namespace _Project.UI
{
    public class SettingsPopupViewPresenter
    {
        private readonly SettingsPopupView _settingsPopupView;
        private readonly IGameDataProvider _gameDataProvider;
        private readonly AudioPlayer _audioPlayer;

        public SettingsPopupViewPresenter(
            PopupButtonView settingsButtonView, 
            SettingsPopupView settingsPopupView, 
            IGameDataProvider gameDataProvider,
            ILocalizationProvider localizationProvider,
            AudioPlayer audioPlayer)
        {
            _settingsPopupView = settingsPopupView;
            _gameDataProvider = gameDataProvider;
            _audioPlayer = audioPlayer;

            settingsButtonView.OnButtonClicked += ShowPopup;
            _settingsPopupView.OnCloseButtonClicked += HidePopup;
            _settingsPopupView.OnMusicSliderChanged += ChangeMusicVolume;
            _settingsPopupView.OnSoundSliderChanged += ChangeSoundVolume;
            
            _settingsPopupView.SetMusicSliderValue(_gameDataProvider.GameDataProxy.MusicVolume.CurrentValue);
            _settingsPopupView.SetSoundSliderValue(_gameDataProvider.GameDataProxy.SoundVolume.CurrentValue);
            
            _settingsPopupView.SetSettingsText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SETTINGS_KEY));
            _settingsPopupView.SetMusicText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.MUSIC_KEY));
            _settingsPopupView.SetSoundText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SOUND_KEY));
            
            _settingsPopupView.Hide();
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
            _audioPlayer.PlayButtonClickSound();
            _settingsPopupView.Show();
        }

        private void HidePopup()
        {
            _audioPlayer.PlayButtonClickSound();
            _settingsPopupView.Hide();
        }
    }
}