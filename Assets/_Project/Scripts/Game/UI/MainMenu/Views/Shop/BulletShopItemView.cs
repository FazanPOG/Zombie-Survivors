using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class BulletShopItemView : MonoBehaviour
    {
        [SerializeField] private Sprite _adSprite;
        [SerializeField] private Sprite _softCurrencySprite;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;

        public event Action OnBuyButtonClicked;

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(() => OnBuyButtonClicked?.Invoke());
        }

        public void SetADPriceImage() => _priceImage.sprite = _adSprite;
        public void SetSoftCurrencyPriceImage() => _priceImage.sprite = _softCurrencySprite;

        public void SetIconImageSprite(Sprite sprite) => _iconImage.sprite = sprite;
        public void SetAmountText(string text) => _amountText.text = text;
        public void SetPriceText(string text) => _priceText.text = text;
        
        private void OnDisable()
        {
            _buyButton.onClick.RemoveAllListeners();
        }
    }
}