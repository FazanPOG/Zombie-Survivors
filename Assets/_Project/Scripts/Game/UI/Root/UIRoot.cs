using _Project.API;
using UnityEngine;

namespace _Project.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _uiSceneRoot;

        public void ShowLoadingScreen() => _loadingScreen.Show();

        public void HideLoadingScreen() => _loadingScreen.Hide();

        public void Init(ILocalizationProvider localizationProvider)
        {
            _loadingScreen.SetGameTitleText(localizationProvider.LocalizationAsset.GetTranslation(LocalizationKeys.GAME_NAME_KEY));
            _loadingScreen.Hide();
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneRoot();
            sceneUI.transform.SetParent(_uiSceneRoot, false);
        }

        private void ClearSceneRoot()
        {
            for (int i = _uiSceneRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(_uiSceneRoot.GetChild(i).gameObject);
            }
        }
    }
}
