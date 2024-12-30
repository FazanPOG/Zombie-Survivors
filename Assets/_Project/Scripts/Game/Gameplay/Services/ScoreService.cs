using _Project.Data;

namespace _Project.Gameplay
{
    public class ScoreService : IScoreService
    {
        private readonly LevelScore _levelScore;
        
        public ScoreService(LevelScore levelScore)
        {
            _levelScore = levelScore;
        }
        
        public void ZombieKilled(ZombieType zombieType)
        {
            _levelScore.Score.Value++;
        }
    }
}