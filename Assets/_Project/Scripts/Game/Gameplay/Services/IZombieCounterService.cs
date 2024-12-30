using R3;

namespace _Project.Gameplay
{
    public interface IZombieCounterService
    {
        ReadOnlyReactiveProperty<int> Count { get; }
        void Add();
        void Remove();
    }
}