using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _filledImage;
        [SerializeField] private TextMeshProUGUI _hpValueText;

        public void SetFilledImageFill(float value) => _filledImage.fillAmount = value;
        public void SetHealthValueText(string text) => _hpValueText.text = text;
    }
}