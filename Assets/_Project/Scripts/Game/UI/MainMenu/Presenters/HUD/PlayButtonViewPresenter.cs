using _Project.Root;
using _Project.Utility;

namespace _Project.UI
{
    public class PlayButtonViewPresenter
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public PlayButtonViewPresenter(PlayButtonView view, ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;

            view.OnButtonClicked += () =>
            {
                sceneLoaderService.LoadSceneAsync(Scenes.Gameplay);
            };
        }
    }
}