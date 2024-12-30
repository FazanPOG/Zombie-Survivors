using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class LoseScreenView : MonoBehaviour
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _adContinueButton;
        [SerializeField] private Button _softCurrencyContinueButton;

        public event Action OnHomeButtonClicked;
        public event Action OnADContinueButtonClicked;
        public event Action OnSoftCurrencyContinueButtonClicked;
        
        private void OnEnable()
        {
            _homeButton.onClick.AddListener(() => OnHomeButtonClicked?.Invoke());
            _adContinueButton.onClick.AddListener(() => OnADContinueButtonClicked?.Invoke());
            _softCurrencyContinueButton.onClick.AddListener(() => OnSoftCurrencyContinueButtonClicked?.Invoke());
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        private void OnDisable()
        {
            _homeButton.onClick.RemoveAllListeners();
            _adContinueButton.onClick.RemoveAllListeners();
            _softCurrencyContinueButton.onClick.RemoveAllListeners();
        }
    }
}