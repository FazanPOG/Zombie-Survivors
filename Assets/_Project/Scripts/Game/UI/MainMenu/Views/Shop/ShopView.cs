using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _bulletButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private GameObject _bulletButtonFocusView;
        [SerializeField] private GameObject _bulletButtonUnFocusView;
        [SerializeField] private GameObject _upgradeButtonFocusView;
        [SerializeField] private GameObject _upgradeButtonUnFocusView;
        [SerializeField] private GameObject _bulletPanel;
        [SerializeField] private GameObject _upgradePanel;

        public event Action OnCloseButtonClicked;
        public event Action OnBulletButtonClicked;
        public event Action OnUpgradeButtonClicked;

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            _bulletButton.onClick.AddListener(() => OnBulletButtonClicked?.Invoke());
            _upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked?.Invoke());
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        public void SetBulletPanelActiveState(bool activeState)
        {
            _bulletButtonFocusView.SetActive(activeState);
            _bulletButtonUnFocusView.SetActive(!activeState);
            _bulletPanel.SetActive(activeState);
        } 
        
        public void SetUpgradePanelActiveState(bool activeState)
        {
            _upgradeButtonFocusView.SetActive(activeState);
            _upgradeButtonUnFocusView.SetActive(!activeState);
            _upgradePanel.SetActive(activeState);
        } 
        
        private void OnDisable()
        {
            _closeButton.onClick.RemoveAllListeners();
            _bulletButton.onClick.RemoveAllListeners();
            _upgradeButton.onClick.RemoveAllListeners();
        }
    }
}