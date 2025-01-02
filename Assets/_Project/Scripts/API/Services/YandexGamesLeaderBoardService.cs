using YG;

namespace _Project.API
{
    public class YandexGamesLeaderBoardService : ILeaderBoardService
    {
        private const string LEADER_BOARD_NAME = "Score";
        
        public void SetNewScore(long newScore)
        {
            YandexGame.NewLeaderboardScores(LEADER_BOARD_NAME, newScore);
        }
    }
}