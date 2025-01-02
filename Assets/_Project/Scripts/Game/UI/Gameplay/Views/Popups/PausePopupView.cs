using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class PausePopupView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _continueButtonText;
        [SerializeField] private TextMeshProUGUI _exitButtonText;

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

        public void SetTitleText(string text) => _titleText.text = text;
        public void SetContinueButtonText(string text) => _continueButtonText.text = text;
        public void SetExitButtonText(string text) => _exitButtonText.text = text;
        
        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            _resumeButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}