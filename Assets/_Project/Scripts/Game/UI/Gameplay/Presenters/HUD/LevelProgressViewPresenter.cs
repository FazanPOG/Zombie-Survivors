using System;
using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.UI
{
    public class LevelProgressViewPresenter : IDisposable
    {
        private readonly LevelProgressView _view;
        private readonly float _maxProgress;

        private IDisposable _disposable;
        
        public LevelProgressViewPresenter(LevelProgress levelProgress, LevelProgressView view)
        {
            _view = view;
            _maxProgress = levelProgress.MaxProgress;
            
            _disposable = levelProgress.Progress.Subscribe(UpdateView);
        }

        private void UpdateView(int progress)
        {
            float progressPercentage = Mathf.Clamp(progress / _maxProgress, 0, 1);
            _view.SetProgressBarFill(progressPercentage);
            _view.SetProgressText($"{progressPercentage * 100}%");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}