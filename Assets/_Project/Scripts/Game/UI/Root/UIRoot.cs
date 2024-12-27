using System;
using UnityEngine;
using VFolders.Libs;

namespace _Project.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _uiSceneRoot;

        public void ShowLoadingScreen() => _loadingScreen.Show();

        public void HideLoadingScreen() => _loadingScreen.Hide();

        private void Start()
        {
            _loadingScreen.Hide();
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneRoot();
            sceneUI.transform.SetParent(_uiSceneRoot, false);
        }

        private void ClearSceneRoot()
        {
            var children = _uiSceneRoot.GetChildren();
            foreach (var child in children)
                Destroy(child.gameObject);
        }
    }
}
