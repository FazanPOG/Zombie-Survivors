using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class PopupButtonTextView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        public event Action OnButtonClicked;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }

        public void SetText(string text) => _text.text = text;
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}