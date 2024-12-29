namespace _Project.Data
{
    public class LevelProgressData
    {
        public int Progress;
        public int MaxProgress;

        public LevelProgressData(int initProgress, int maxProgress)
        {
            Progress = initProgress;
            MaxProgress = maxProgress;
        }
    }
}