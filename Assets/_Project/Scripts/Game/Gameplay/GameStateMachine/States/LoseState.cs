using _Project.API;
using _Project.Data;

namespace _Project.Gameplay
{
    public class LoseState : IGameState
    {
        private readonly LevelScore _levelScore;
        private readonly ILeaderBoardService _leaderBoardService;
        private readonly IGameDataProvider _gameDataProvider;

        public LoseState(LevelScore levelScore, ILeaderBoardService leaderBoardService, IGameDataProvider gameDataProvider)
        {
            _levelScore = levelScore;
            _leaderBoardService = leaderBoardService;
            _gameDataProvider = gameDataProvider;
        }
        
        public void Enter()
        {
            int bestScore = _gameDataProvider.GameDataProxy.BestScore.CurrentValue;
            int currentScore = _levelScore.Score.Value;
            
            if (bestScore < currentScore)
            {
                _leaderBoardService.SetNewScore(currentScore);
                _gameDataProvider.GameDataProxy.BestScore.Value = currentScore;
            }
            
            _gameDataProvider.GameDataProxy.SoftCurrency.Value += currentScore;
            
            _gameDataProvider.SaveGameData();
        }

        public void Exit()
        {
        }
    }
}