using R3;

namespace _Project.Data
{
    public class PlayerHealth
    {
        private ReactiveProperty<int> _health = new ReactiveProperty<int>();
            
        public ReactiveProperty<int> Health => _health;
        public int MaxHealth { get; }
        
        public PlayerHealth(PlayerHealthData healthData)
        {
            _health.Value = healthData.Value;
            MaxHealth = healthData.Value;
            
            _health.Subscribe(newValue =>
            {
                if(newValue <= 0)
                    healthData.Value = 0;
                else
                    healthData.Value = newValue;
            });
        }
    }
}