using System;
using UnityEngine;
using UnityEngine.UI;

namespace Factura.Health
{
    public interface IBaseHealth
    {
        event Action OnDied;
        event Action<float> OnHealthNormalizedChanged;

        void TakeDamage(int damage);
        void ResetHealth();
        void IsActiveSlider(bool isActive);
    }

    public class BaseHealth : MonoBehaviour, IBaseHealth
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private Slider _healthSlider;

        public event Action OnDied;
        public event Action<float> OnHealthNormalizedChanged;

        private event Action<int> OnHealthChanged;
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            UpdateSlider();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            OnHealthChanged?.Invoke(_currentHealth);
            OnHealthNormalizedChanged?.Invoke((float)_currentHealth / _maxHealth);
            UpdateSlider();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth);
            OnHealthNormalizedChanged?.Invoke(1f);
            UpdateSlider();
        }

        public void IsActiveSlider(bool isActive)
        {
            _healthSlider.gameObject.SetActive(isActive);
        }

        private void UpdateSlider()
        {
            if (_healthSlider != null)
            {
                _healthSlider.value = (float)_currentHealth / _maxHealth;
            }
        }

        private void Die()
        {
            OnDied?.Invoke();
        }
    }
}