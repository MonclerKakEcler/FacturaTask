using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Factura.Turret
{
    public interface ITurretView : IDragHandler, IEndDragHandler
    {
        event Action<float> OnTurretRotateInput;
        event Action OnShootStart;
        event Action OnShootStop;
        Transform TurretTransform { get; }
        Transform FirePlaceTransform { get; }
        ParticleSystem SmokePS { get; }
    }

    public class TurretView : MonoBehaviour, ITurretView
    {
        [SerializeField] private Transform _turrentTrasform;
        [SerializeField] private Transform _firePlaceTrasform;
        [SerializeField] private ParticleSystem _smokePS;

        public event Action<float> OnTurretRotateInput;
        public event Action OnShootStart;
        public event Action OnShootStop;

        public Transform TurretTransform => _turrentTrasform;
        public Transform FirePlaceTransform => _firePlaceTrasform;
        public ParticleSystem SmokePS => _smokePS;

        public void OnDrag(PointerEventData eventData)
        {
            float deltaX = eventData.delta.x;
            OnTurretRotateInput?.Invoke(deltaX);
            OnShootStart?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnShootStop?.Invoke();
        }
    }
}