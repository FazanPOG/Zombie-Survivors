using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Audio
{
    public class AudioPlayer
    {
        private readonly AudioSource _audioSource;
        private readonly CommonAudioClipsConfig _commonAudioClipsConfig;

        public AudioPlayer(AudioSource audioSource, IGameDataProvider gameDataProvider, CommonAudioClipsConfig commonAudioClipsConfig)
        {
            _audioSource = audioSource;
            _commonAudioClipsConfig = commonAudioClipsConfig;

            gameDataProvider.GameDataProxy.SoundVolume.Subscribe(ChangeVolume);
        }

        public void PlaySoundOneShot(AudioClip clip, float volumeScale = 1f)
        {
            _audioSource.PlayOneShot(clip, volumeScale);
        }

        public void PlayButtonClickSound(float volumeScale = 1f)
        {
            _audioSource.PlayOneShot(_commonAudioClipsConfig.ButtonClicked, volumeScale);
        }
        
        private void ChangeVolume(float volume) => _audioSource.volume = volume;
    }
}