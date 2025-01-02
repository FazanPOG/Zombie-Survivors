using _Project.API;
using _Project.Root;
using _Project.Utility;

namespace _Project.UI
{
    public class PlayButtonViewPresenter
    {
        public PlayButtonViewPresenter(PlayButtonView view, ISceneLoaderService sceneLoaderService, ILocalizationProvider localizationProvider)
        {
            view.SetPlayText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.PLAY_KEY));
            
            view.OnButtonClicked += () =>
            {
                sceneLoaderService.LoadSceneAsync(Scenes.Gameplay);
            };
        }
    }
}