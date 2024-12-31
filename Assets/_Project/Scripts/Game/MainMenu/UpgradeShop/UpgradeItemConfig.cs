using _Project.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.MainMenu
{
    [CreateAssetMenu(menuName = "_Project/MainMenu/UpgradeItem")]
    public class UpgradeItemConfig : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private string _id;
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField, Min(1)] private int _defaultPrice = 1;
        [SerializeField, Min(1)] private float _increasePriceCoefficient = 1f;
        [SerializeField, Min(1)] private float _increaseUpgradeValueCoefficient = 1f;
        [SerializeField, Min(1)] private int _maxUpgradeLevel = 1;

        [Header("View")]
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private Sprite _itemIconBackgroundImage;
        [SerializeField] private string _itemNameText;
        [SerializeField] private string _itemDescriptionText;

        public string ID => _id;

        public UpgradeType UpgradeType => _upgradeType;

        public CurrencyType CurrencyType => _currencyType;

        public int DefaultPrice => _defaultPrice;

        public float IncreasePriceCoefficient => _increasePriceCoefficient;

        public float IncreaseUpgradeValueCoefficient => _increaseUpgradeValueCoefficient;

        public int MaxUpgradeLevel => _maxUpgradeLevel;

        public Sprite ItemIcon => _itemIcon;

        public Sprite ItemIconBackgroundSprite => _itemIconBackgroundImage;

        public string ItemNameText => _itemNameText;

        public string ItemDescriptionText => _itemDescriptionText;
    }
}