using R3;
using UnityEngine;

namespace _Project.Gameplay
{
    public class PlayerMovement
    {
        private const float _maxSpeed = 4f;

        private readonly Transform _playerTransform;
        private readonly IInput _input;
        private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();

        private float _moveSpeed;
        private float _defaultMoveSpeed;

        public float Speed => _moveSpeed;
        public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;

        public PlayerMovement(Transform playerTransform, IInput input)
        {
            _playerTransform = playerTransform;
            _input = input;
        }
        
        public void Update()
        {
            MovementHandler();
        }

        private void MovementHandler()
        {
            Vector2 inputVector = _input.MovementInput.normalized;

            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            _moveSpeed = _maxSpeed * moveDir.magnitude;

            float moveDistance = _moveSpeed * Time.deltaTime;
            float playerRadius = .3f;
            float playerHeight = 2f;
            bool canMove = !Physics.CapsuleCast(new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1f, _playerTransform.position.z) , _playerTransform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
            Debug.DrawRay(_playerTransform.position + Vector3.up * playerHeight, moveDir, Color.green, moveDistance, false);

            if (!canMove)
            {
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(_playerTransform.position, _playerTransform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                Debug.DrawRay(_playerTransform.position, moveDirX, Color.yellow, moveDistance, false);
                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(_playerTransform.position, _playerTransform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                    else
                    {
                        //can't move on any direction
                    }
                }
            }
            if (canMove)
            {
                _playerTransform.position += moveDir * _moveSpeed * Time.deltaTime;
            }
            
            _isMoving.Value = moveDir != Vector3.zero;
            float rotateSpeed = 10f;
            _playerTransform.forward = Vector3.Slerp(_playerTransform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
}
