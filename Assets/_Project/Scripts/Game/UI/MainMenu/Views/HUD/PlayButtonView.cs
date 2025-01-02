using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class PlayButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _playText;

        public event Action OnButtonClicked;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }

        public void SetPlayText(string text) => _playText.text = text;
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}