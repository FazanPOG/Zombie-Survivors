using System;
using _Project.Data;
using R3;

namespace _Project.UI
{
    public class LevelScoreViewPresenter : IDisposable
    {
        private readonly LevelScoreView _view;
        private readonly IDisposable _disposable;
        
        public LevelScoreViewPresenter(LevelScore levelScore, LevelScoreView view)
        {
            _view = view;
            
            _disposable = levelScore.Score.Subscribe(UpdateView);
        }

        private void UpdateView(int score)
        {
            _view.SetScoreText($"Score {score}");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}