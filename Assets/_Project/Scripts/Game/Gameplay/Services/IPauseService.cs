namespace _Project.Gameplay
{
    public interface IPauseService
    {
        void SetPause(bool isPaused);
        void Register(IPauseHandler pauseHandler);
        void Unregister(IPauseHandler pauseHandler);
    }
}