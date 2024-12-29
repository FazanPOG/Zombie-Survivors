using System;
using R3;

namespace _Project.Data
{
    public class LevelProgress
    {
        private ReactiveProperty<int> _levelProgress = new ReactiveProperty<int>();

        public ReactiveProperty<int> Progress => _levelProgress;

        public int MaxProgress { get; }

        public LevelProgress(LevelProgressData levelProgressData)
        {
            _levelProgress.Value = levelProgressData.Progress;
            MaxProgress = levelProgressData.MaxProgress;
            
            _levelProgress.Subscribe(newValue =>
            {
                if(newValue < 0)
                    throw new ArgumentOutOfRangeException();
                
                if(newValue >= MaxProgress)
                    levelProgressData.Progress = MaxProgress;
                else
                    levelProgressData.Progress = newValue;
            });
        }
    }
}