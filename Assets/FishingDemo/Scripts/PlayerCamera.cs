using UnityEngine;
using UnityEngine.InputSystem;

namespace Lakeside.Fishing
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] float sensitivity = .12f;
        [SerializeField] float pitchLimit = 85f;
        [SerializeField] bool headBob = true;
        [SerializeField] float bobAmount = .025f;
        Transform view;
        PlayerMovement movement;
        Vector3 restingPosition;
        float pitch, bobTime;
        public void Initialize(Transform cameraView, PlayerMovement playerMovement)
        {
            view = cameraView;
            movement = playerMovement;
            restingPosition = view.localPosition;
        }
        void LateUpdate()
        {
            if (view == null || movement == null || !movement.AcceptsInput) return;
            if (Mouse.current != null)
            {
                Vector2 look = Mouse.current.delta.ReadValue() * sensitivity;
                pitch = Mathf.Clamp(pitch - look.y, -pitchLimit, pitchLimit);
                transform.Rotate(0, look.x, 0);
            }
            view.localRotation = Quaternion.Euler(pitch, 0, 0);
            Vector3 offset = Vector3.zero;
            if (headBob && movement.Grounded && movement.Speed > .1f)
            {
                bobTime += Time.deltaTime * (movement.IsSprinting ? 14 : 10);
                offset.y = Mathf.Sin(bobTime) * bobAmount;
            }
            else bobTime = 0;
            view.localPosition = Vector3.Lerp(view.localPosition, restingPosition + offset, 1 - Mathf.Exp(-15 * Time.deltaTime));
        }
    }
}
