using TMPro;
using UnityEngine;

namespace _Project.UI
{
    public class ClickToStartView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _clickText;

        public void SetClickText(string text) => _clickText.text = text;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}