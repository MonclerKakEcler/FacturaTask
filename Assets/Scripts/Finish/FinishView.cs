using UnityEngine;
using System;
using Factura.Car;

namespace Factura.Finish
{
    public class FinishView : MonoBehaviour
    {
        public event Action OnLevelFinished;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICarView>(out _))
            {
                OnLevelFinished?.Invoke();
            }
        }
    }
}