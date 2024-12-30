using System;
using R3;

namespace _Project.Data
{
    public class LevelScore
    {
        private ReactiveProperty<int> _levelScore = new ReactiveProperty<int>();

        public ReactiveProperty<int> Score => _levelScore;

        public int MaxProgress { get; }

        public LevelScore(LevelScoreData levelScoreData)
        {
            _levelScore.Value = levelScoreData.Progress;
            MaxProgress = levelScoreData.MaxProgress;
            
            _levelScore.Subscribe(newValue =>
            {
                if(newValue < 0)
                    throw new ArgumentOutOfRangeException();
                
                if(newValue >= MaxProgress)
                    levelScoreData.Progress = MaxProgress;
                else
                    levelScoreData.Progress = newValue;
            });
        }
    }
}