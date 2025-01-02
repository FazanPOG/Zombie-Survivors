using TMPro;
using UnityEngine;

namespace _Project.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _gameTitleText;
        
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetGameTitleText(string text) => _gameTitleText.text = text;
    }
}