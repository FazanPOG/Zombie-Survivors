using _Project.Gameplay;
using UnityEngine;

namespace _Project.Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "_Project/Data/DefaultDataConfig")]
    public class DefaultDataConfig : ScriptableObject
    {
        [Header("Player Data")]
        [SerializeField, Min(1)] private int _health = 1;
        [SerializeField, Range(0.1f, 20f)] private float _moveSpeed = 1f;
        [Header("Default Environments")]
        [SerializeField] private Environment _environment;
        [Header("Audio")]
        [SerializeField, Range(0f, 1f)] private float _musicVolume;
        [SerializeField, Range(0f, 1f)] private float _soundVolume;
        [SerializeField] private AudioClip _backgroundMusic;
        
        public int PlayerHealth => _health;
        public float PlayerMoveSpeed => _moveSpeed;

        public Environment Environment => _environment;
        
        public float MusicVolume => _musicVolume;

        public float SoundVolume => _soundVolume;

        public AudioClip BackgroundMusic => _backgroundMusic;
    }
}