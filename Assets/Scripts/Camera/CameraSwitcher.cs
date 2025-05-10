using Cinemachine;
using UnityEngine;

namespace Factura.Camera
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _previewCamera;

        public void IsActivePreviewCamera(bool isActive)
        {
            _previewCamera.gameObject.SetActive(isActive);
        }
    }
}