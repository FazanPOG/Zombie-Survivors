using TMPro;
using UnityEngine;

namespace _Project.UI
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyText;

        public void SetCurrencyText(string text) => _currencyText.text = text;
    }
}