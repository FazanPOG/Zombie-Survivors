using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class PopupButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action OnButtonClicked; 
        
        private void OnEnable()
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}