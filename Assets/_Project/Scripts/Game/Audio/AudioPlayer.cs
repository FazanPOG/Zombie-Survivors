using UnityEngine;

namespace _Project.Game
{
    public class AudioPlayer
    {
        private readonly AudioSource _audioSource;

        public AudioPlayer(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void PlaySound(AudioClip clip, float volumeScale = 1f)
        {
            _audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}