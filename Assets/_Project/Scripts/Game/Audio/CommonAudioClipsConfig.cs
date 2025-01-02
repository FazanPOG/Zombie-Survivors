using UnityEngine;

namespace _Project.Audio
{
    [CreateAssetMenu(menuName = "_Project/Audio/CommonClips", order = 0)]
    public class CommonAudioClipsConfig : ScriptableObject
    {
        [SerializeField] private AudioClip _buttonClicked;

        public AudioClip ButtonClicked => _buttonClicked;
    }
}