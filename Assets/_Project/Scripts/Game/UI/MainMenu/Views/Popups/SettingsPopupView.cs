﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class SettingsPopupView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        public event Action OnCloseButtonClicked;
        public event Action<float> OnMusicSliderChanged;
        public event Action<float> OnSoundSliderChanged;
        
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            _musicSlider.onValueChanged.AddListener((value) => OnMusicSliderChanged?.Invoke(value));
            _soundSlider.onValueChanged.AddListener((value) => OnSoundSliderChanged?.Invoke(value));
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetMusicSliderValue(float value) => _musicSlider.value = value;
        public void SetSoundSliderValue(float value) => _soundSlider.value = value;
        
        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            _soundSlider.onValueChanged.RemoveAllListeners();
        }
    }
}