using R3;

namespace _Project.Data
{
    public class PlayerHealth
    {
        private ReactiveProperty<int> _health = new ReactiveProperty<int>();
            
        public ReactiveProperty<int> Health => _health;
        
        public PlayerHealth(PlayerHealthData moveSpeedData)
        {
            _health.Value = moveSpeedData.Value;

            _health.Subscribe(newValue =>
            {
                if(newValue <= 0)
                    moveSpeedData.Value = 0;
                else
                    moveSpeedData.Value = newValue;
            });
        }
    }
}