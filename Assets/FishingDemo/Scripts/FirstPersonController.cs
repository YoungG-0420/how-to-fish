using UnityEngine;

namespace Lakeside.Fishing
{
    /// <summary>Connects the scene bootstrap to the standalone player controls.</summary>
    public class FirstPersonController : MonoBehaviour
    {
        public void Initialize(Transform cameraView)
        {
            var movement = GetComponent<PlayerMovement>() ?? gameObject.AddComponent<PlayerMovement>();
            var cameraControl = GetComponent<PlayerCamera>() ?? gameObject.AddComponent<PlayerCamera>();
            movement.Initialize(cameraView.parent);
            cameraControl.Initialize(cameraView, movement);
        }
    }
}
