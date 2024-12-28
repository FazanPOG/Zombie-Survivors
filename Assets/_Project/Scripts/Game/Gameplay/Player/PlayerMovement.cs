using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class PlayerMovement
    {
        private readonly Transform _playerTransform;
        private readonly IInput _input;
        private readonly ReadOnlyReactiveProperty<float> _maxMoveSpeed;
        private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();

        private float _moveSpeed;
        private float _defaultMoveSpeed;
        private bool _isMovementEnabled;
        private Vector3 _moveDir;

        public float Speed => _moveSpeed;

        public Vector3 MoveDirection => _moveDir;
        public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

        public PlayerMovement(Transform playerTransform, IInput input, ReadOnlyReactiveProperty<float> maxMoveSpeed)
        {
            _playerTransform = playerTransform;
            _input = input;
            _maxMoveSpeed = maxMoveSpeed;
        }

        public void EnableMovement() => _isMovementEnabled = true;

        public void DisableMovement() => _isMovementEnabled = false;

        public void Update()
        {
            if(_isMovementEnabled)
                Move();
        }

        private void Move()
        {
            Vector2 inputVector = _input.MovementInput.normalized;

            _moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            _moveSpeed = _maxMoveSpeed.CurrentValue * _moveDir.magnitude;

            float moveDistance = _moveSpeed * Time.deltaTime;
            float playerRadius = .3f;
            float playerHeight = 2f;
            bool canMove = !Physics.CapsuleCast(new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1f, _playerTransform.position.z) , _playerTransform.position + Vector3.up * playerHeight, playerRadius, _moveDir, moveDistance);
            Debug.DrawRay(_playerTransform.position + Vector3.up * playerHeight, _moveDir, Color.green, moveDistance, false);

            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(_moveDir.x, 0, 0).normalized;
                canMove = (_moveDir.x < -.5f || _moveDir.x > +.5f) && !Physics.CapsuleCast(_playerTransform.position, _playerTransform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                Debug.DrawRay(_playerTransform.position, moveDirX, Color.yellow, moveDistance, false);
                if (canMove)
                {
                    _moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, _moveDir.z).normalized;
                    canMove = (_moveDir.z < -.5f || _moveDir.z > +.5f) && !Physics.CapsuleCast(_playerTransform.position, _playerTransform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {
                        _moveDir = moveDirZ;
                    }
                    else
                    {
                        //can't move on any direction
                    }
                }
            }
            if (canMove)
            {
                _playerTransform.position += _moveDir * (_moveSpeed * Time.deltaTime);
            }
            
            _isMoving.Value = _moveDir != Vector3.zero;
        }
    }
}
