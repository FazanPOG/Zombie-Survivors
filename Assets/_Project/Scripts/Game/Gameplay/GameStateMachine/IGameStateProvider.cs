using R3;

namespace _Project.Gameplay
{
    public interface IGameStateProvider
    {
        ReadOnlyReactiveProperty<IGameState> GameState { get; }
    }
}