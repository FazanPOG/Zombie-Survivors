using System;
using R3;

namespace _Project.Data
{
    public class LevelScore
    {
        private ReactiveProperty<int> _levelScore = new ReactiveProperty<int>();

        public ReactiveProperty<int> Score => _levelScore;

        public LevelScore(LevelScoreData levelScoreData)
        {
            _levelScore.Value = levelScoreData.Value;
            
            _levelScore.Subscribe(newValue =>
            {
                if(newValue < 0)
                    throw new ArgumentOutOfRangeException();
                
                levelScoreData.Value = newValue;
            });
        }
    }
}