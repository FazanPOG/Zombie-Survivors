using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Audio
{
    public class BackgroundMusic
    {
        private readonly AudioSource _audioSource;
        private readonly IGameDataProvider _gameDataProvider;

        public BackgroundMusic(AudioSource audioSource, IGameDataProvider gameDataProvider)
        {
            _audioSource = audioSource;

            gameDataProvider.GameDataProxy.MusicVolume.Subscribe(ChangeVolume);
        }

        private void ChangeVolume(float volume) => _audioSource.volume = volume;

        public void PlayMusic(AudioClip clip, float volumeScale = 1f)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Play();
        }
    }
}