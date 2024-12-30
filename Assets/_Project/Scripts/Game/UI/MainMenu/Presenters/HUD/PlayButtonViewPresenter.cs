using _Project.Root;
using _Project.Utility;

namespace _Project.UI
{
    public class PlayButtonViewPresenter
    {
        public PlayButtonViewPresenter(PlayButtonView view, ISceneLoaderService sceneLoaderService)
        {
            view.OnButtonClicked += () =>
            {
                sceneLoaderService.LoadSceneAsync(Scenes.Gameplay);
            };
        }
    }
}