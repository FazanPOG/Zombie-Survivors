using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class PausePopupView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;

        public event Action OnCloseButtonClicked;
        public event Action OnResumeButtonClicked;
        public event Action OnExitButtonClicked;
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            _resumeButton.onClick.AddListener(() => OnResumeButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}