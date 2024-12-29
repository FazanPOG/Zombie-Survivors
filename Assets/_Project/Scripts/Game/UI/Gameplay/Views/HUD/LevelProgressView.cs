using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _progressText;

        public void SetProgressText(string text) => _progressText.text = text;
        public void SetProgressBarFill(float value) => _bar.fillAmount = value;
    }
}