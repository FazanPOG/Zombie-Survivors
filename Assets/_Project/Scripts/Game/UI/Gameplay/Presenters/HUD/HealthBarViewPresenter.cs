using System;
using _Project.Data;
using R3;
using UnityEngine;

namespace _Project.UI
{
    public class HealthBarViewPresenter : IDisposable
    {
        private readonly HealthBarView _view;
        private readonly IDisposable _disposable;
        private readonly float _maxHP;
        
        public HealthBarViewPresenter(HealthBarView view, PlayerHealth playerHealth)
        {
            _view = view;
            _maxHP = playerHealth.Health.CurrentValue;
            
            _disposable = playerHealth.Health.Subscribe(UpdateView);
        }

        private void UpdateView(int hp)
        {
            _view.SetFilledImageFill(Mathf.Clamp(hp / _maxHP, 0, 1));
            _view.SetHealthValueText($"{hp}/{_maxHP}");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}