using UnityEngine;

namespace antonkesy
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Vector2 targetAspect = new Vector2(9, 16);
        private float _targetAspectRatio;
        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
            _targetAspectRatio = targetAspect.y / targetAspect.x;

            AdjustFOVToFitScreenToCamera();
        }

        private void AdjustFOVToFitScreenToCamera()
        {
            var aspectRatio = (float)Screen.height / Screen.width;
#if UNITY_EDITOR
            Debug.Log($"FOV {_camera.fieldOfView}");

            Debug.Log(
                $"Width {Screen.width}\tHeight {Screen.height}\tAspect {aspectRatio}\ttargetAspect {_targetAspectRatio}");
#endif
            var aspectDif = aspectRatio - _targetAspectRatio;
            if (Mathf.Abs(aspectDif) > 0.1f)
            {
#if UNITY_EDITOR
                Debug.Log("Resize");
#endif
                _camera.fieldOfView += 10f * aspectDif;
            }
        }
    }
}