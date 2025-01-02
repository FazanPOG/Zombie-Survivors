using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.MainMenu
{
    [CreateAssetMenu(menuName = "_Project/MainMenu/BulletItemConfig")]
    public class BulletItemConfig : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private string _id;
        [SerializeField] private BulletItemPriceType _priceType;
        [SerializeField, Min(1)] private int _bulletAmount;
        [SerializeField, Min(1), ShowIf(nameof(_isAdPriceType))] private int _adAmount;
        [SerializeField, Min(1), ShowIf(nameof(_isSoftCurrencyPriceType))] private int _softCurrencyPrice;
        [Header("View")]
        [SerializeField] private Sprite _iconSprite;

        private bool _isAdPriceType;
        private bool _isSoftCurrencyPriceType;

        public string ID => _id;
        
        public BulletItemPriceType PriceType => _priceType;

        public int BulletAmount => _bulletAmount;

        public int ADAmount => _adAmount;

        public int SoftCurrencyPrice => _softCurrencyPrice;
        
        public Sprite IconSprite => _iconSprite;
        
        private void OnValidate()
        {
            _isAdPriceType = _priceType == BulletItemPriceType.AD;
            _isSoftCurrencyPriceType = _priceType == BulletItemPriceType.SoftCurrency;
        }
    }
}