using R3;

namespace _Project.Gameplay
{
    public class BoostCounterService : IBoostCounterService
    {
        private readonly ReactiveProperty<int> _count = new ReactiveProperty<int>();

        public ReadOnlyReactiveProperty<int> Count => _count;
        
        public void Add()
        {
            _count.Value++;
        }

        public void Remove()
        {
            _count.Value--;
        }
    }
}