using Cinemachine;
using UnityEngine;

namespace _Project.Gameplay
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public void Init(Transform followTarget)
        {
            _virtualCamera.Follow = followTarget;
            _virtualCamera.LookAt = followTarget;
        }
    }
}