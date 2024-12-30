using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.Audio
{
    public class AudioPlayer
    {
        private readonly AudioSource _audioSource;

        public AudioPlayer(AudioSource audioSource, IGameDataProvider gameDataProvider)
        {
            _audioSource = audioSource;
            
            gameDataProvider.GameDataProxy.SoundVolume.Subscribe(ChangeVolume);
        }

        private void ChangeVolume(float volume) => _audioSource.volume = volume;
        
        public void PlaySoundOneShot(AudioClip clip, float volumeScale = 1f)
        {
            _audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}