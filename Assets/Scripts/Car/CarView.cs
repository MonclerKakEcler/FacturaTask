using System;
using UnityEngine;
using UnityEngine.UI;
using Factura.Health;

namespace Factura.Car
{
    public interface ICarView
    {
        event Action OnHitByEnemy;
        Transform CarTransform { get; }
        Vector3 CurrentPosition { get; }
        BaseHealth Health { get; }
        void SetActiveLosePanel(bool isActive);
        void InvokeHitByEnemy();
    }

    public class CarView : MonoBehaviour, ICarView
    {
        [SerializeField] private Image _loseScreen;
        [SerializeField] private Transform _carTransform;
        [SerializeField] private BaseHealth _health;

        public Transform CarTransform => _carTransform;
        public Vector3 CurrentPosition => _carTransform.position;
        public BaseHealth Health => _health;

        public event Action OnHitByEnemy;

        public void InvokeHitByEnemy()
        {
            OnHitByEnemy?.Invoke();
        }

        public void SetActiveLosePanel(bool isActive)
        {
            _loseScreen.gameObject.SetActive(isActive);
        }
    }
}
