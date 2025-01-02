using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class UpgradeShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconBackgroundImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI[] _currentLevelTexts;
        [SerializeField] private TextMeshProUGUI _nextLevelText;
        [SerializeField] private TextMeshProUGUI _currentValueText;
        [SerializeField] private TextMeshProUGUI _nextValueText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _nextLevelArrowImage;
        [SerializeField] private Image _nextValueArrowImage;
        [SerializeField] private Image _upgradeButtonImage;
        [SerializeField] private Button _upgradeButton;

        public event Action OnUpgradeButtonClicked;
        
        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked?.Invoke());
        }

        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;
        public void SetIconBackground(Sprite sprite) => _iconBackgroundImage.sprite = sprite;
        public void SetNameText(string text) => _nameText.text = text;

        public void SetCurrentLevelTexts(string text)
        {
            foreach (var levelText in _currentLevelTexts)
                levelText.text = text;
        }
        
        public void SetNextLevelText(string text) => _nextLevelText.text = text;
        public void SetCurrentValueText(string text) => _currentValueText.text = text;
        public void SetNextValueText(string text) => _nextValueText.text = text;
        public void SetPriceText(string text) => _priceText.text = text;
        public void SetNextLevelTextActiveState(bool activeState) => _nextLevelText.gameObject.SetActive(activeState);
        public void SetNextValueTextActiveState(bool activeState) => _nextValueText.gameObject.SetActive(activeState);
        public void SetNextLevelArrowImageActiveState(bool activeState) => _nextLevelArrowImage.gameObject.SetActive(activeState);
        public void SetNextValueArrowImageActiveState(bool activeState) => _nextValueArrowImage.gameObject.SetActive(activeState);
        public void SetUpgradeButtonImageActiveState(bool activeState) => _upgradeButtonImage.gameObject.SetActive(activeState);
        
        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveAllListeners();
        }
    }
}