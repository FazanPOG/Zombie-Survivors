namespace _Project.Data
{
    public class LevelScoreData
    {
        public int Progress;
        public int MaxProgress;

        public LevelScoreData(int initProgress, int maxProgress)
        {
            Progress = initProgress;
            MaxProgress = maxProgress;
        }
    }
}