using R3;

namespace _Project.Gameplay
{
    public interface IBoostCounterService
    {
        ReadOnlyReactiveProperty<int> Count { get; }
        void Add();
        void Remove();
    }
}