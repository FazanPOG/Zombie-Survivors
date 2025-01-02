using System;
using _Project.API;
using _Project.Data;
using R3;

namespace _Project.UI
{
    public class LevelScoreViewPresenter : IDisposable
    {
        private readonly LevelScoreView _view;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly IDisposable _disposable;
        
        public LevelScoreViewPresenter(LevelScore levelScore, LevelScoreView view, ILocalizationProvider localizationProvider)
        {
            _view = view;
            _localizationProvider = localizationProvider;

            _disposable = levelScore.Score.Subscribe(UpdateView);
        }

        private void UpdateView(int score)
        {
            string scoreText = _localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.SCORE_KEY);
            _view.SetScoreText($"{scoreText} {score}");
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}