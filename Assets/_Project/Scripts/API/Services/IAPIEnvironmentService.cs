namespace _Project.API
{
    public interface IAPIEnvironmentService
    {
        bool IsReady { get; }
        void GameLoadingAndReady();
        void GameplayStart();
        void GameplayStop();
        void SetPaused(bool isPaused);
    }
}