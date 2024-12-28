using UnityEngine;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class PlayerView : MonoBehaviour
    {
        private const string MOVE_SPEED_KEY = "MoveSpeed";
        private const string IS_MELEE_KEY = "IsMelee";
        private const string IS_PISTOL_KEY = "IsPistol";
        private const string IS_RIFLE_KEY = "IsRifle";
        
        private Animator _animator;
        private PlayerMovement _playerMovement;

        private void Awake() => _animator = GetComponent<Animator>();

        public void Init(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
            
            //TODO
            _animator.SetBool(IS_PISTOL_KEY, true);
        }

        private void Update()
        {
            _animator.SetFloat(MOVE_SPEED_KEY, _playerMovement.Speed);
        }
    }
}