using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class LoseScreenView : MonoBehaviour
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _adContinueButton;
        [SerializeField] private Button _softCurrencyContinueButton;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _continueText;
        [SerializeField] private TextMeshProUGUI _adButtonText;
        [SerializeField] private TextMeshProUGUI _softCurrecyValueText;

        public event Action OnHomeButtonClicked;
        public event Action OnADContinueButtonClicked;
        public event Action OnCurrencyContinueButtonClicked;
        
        private void OnEnable()
        {
            _homeButton.onClick.AddListener(() => OnHomeButtonClicked?.Invoke());
            _adContinueButton.onClick.AddListener(() => OnADContinueButtonClicked?.Invoke());
            _softCurrencyContinueButton.onClick.AddListener(() => OnCurrencyContinueButtonClicked?.Invoke());
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetCurrencyContinueButtonActiveState(bool activeState) =>
            _softCurrencyContinueButton.gameObject.SetActive(activeState);
        
        public void SetCurrencyValueText(string text) => _softCurrecyValueText.text = text;
        public void SetTitleText(string text) => _titleText.text = text;
        public void SetContinueText(string text) => _continueText.text = text;
        public void SetADButtonText(string text) => _adButtonText.text = text;
        
        private void OnDisable()
        {
            _homeButton.onClick.RemoveAllListeners();
            _adContinueButton.onClick.RemoveAllListeners();
            _softCurrencyContinueButton.onClick.RemoveAllListeners();
        }
    }
}