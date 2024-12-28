namespace _Project.Gameplay
{
    public interface IGameStateMachine
    {
        void EnterIn<T>() where T : IGameState;
    }
}