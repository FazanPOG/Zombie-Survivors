using _Project.Audio;
using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class BaseBoost : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        
        private BoxCollider _collider;
        private Player _player;
        private AudioPlayer _audioPlayer;
        private IBoostCounterService _boostCounterService;
        private bool _isInit;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        public void Init(AudioPlayer audioPlayer, IBoostCounterService boostCounterService)
        {
            _audioPlayer = audioPlayer;
            _boostCounterService = boostCounterService;
            _isInit = true;
        }
        
        private void Update()
        {
            if(_isInit == false)
                return;
            
            if (_player != null)
            {
                if (Vector3.Distance(_player.transform.position, transform.position) < 1f)
                {
                    _player.TakeBoost(this);
                    _audioPlayer.PlaySoundOneShot(_audioClip);
                    DestroySelf();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                _player = player;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                _player = null;
        }

        private void DestroySelf()
        {
            _boostCounterService.Remove();
            Destroy(gameObject);
        }
    }
}