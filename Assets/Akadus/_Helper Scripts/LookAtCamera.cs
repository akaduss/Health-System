using UnityEngine;

namespace Akadus
{
    public class LookAtCamera : MonoBehaviour
    {
        private enum CameraMode
        {
            LookAt,
            CameraForward
        }

        [SerializeField] private CameraMode _cameraMode;

        void LateUpdate()
        {
            switch (_cameraMode)
            {
                case CameraMode.LookAt:
                    transform.LookAt(transform.position + transform.position - Camera.main.transform.position);
                    break;
                case CameraMode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
            }
        }
    }
}
