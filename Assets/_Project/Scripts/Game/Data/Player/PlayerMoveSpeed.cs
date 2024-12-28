using R3;

namespace _Project.Data
{
    public class PlayerMoveSpeed
    {
        private ReactiveProperty<float> _moveSpeed = new ReactiveProperty<float>();
            
        public ReactiveProperty<float> MoveSpeed => _moveSpeed;
        
        public PlayerMoveSpeed(PlayerMoveSpeedData moveSpeedData)
        {
            _moveSpeed.Value = moveSpeedData.Value;

            _moveSpeed.Subscribe(newValue => moveSpeedData.Value = newValue);
        }
    }
}