namespace _Project.Gameplay
{
    public class BootState : IGameState
    {
        private readonly IPauseService _pauseService;

        public BootState(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }
        
        public void Enter()
        {
            _pauseService.SetPause(true);
        }

        public void Exit()
        {
            _pauseService.SetPause(false);
        }
    }
}