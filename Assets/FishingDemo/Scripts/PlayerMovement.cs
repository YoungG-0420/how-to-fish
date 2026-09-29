using UnityEngine;
using UnityEngine.InputSystem;

namespace Lakeside.Fishing
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float walkSpeed = 5f, sprintSpeed = 8f, acceleration = 22f;
        [SerializeField] float jumpHeight = 1.2f, gravity = -22f;
        [SerializeField, Range(.3f, .9f)] float crouchRatio = .6f;
        CharacterController controller;
        Transform head;
        Vector3 standingHeadPosition, horizontalVelocity;
        float standingHeight, verticalVelocity;
        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool Grounded => controller != null && controller.isGrounded;
        public float Speed => controller == null ? 0 : new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        public bool AcceptsInput => head != null && Application.isFocused && Cursor.lockState == CursorLockMode.Locked;

        public void Initialize(Transform headAnchor)
        {
            controller = GetComponent<CharacterController>();
            head = headAnchor;
            standingHeadPosition = head.localPosition;
            standingHeight = Mathf.Max(controller.height, standingHeadPosition.y + .45f);
            controller.height = standingHeight;
            controller.center = Vector3.up * standingHeight * .5f;
        }

        void Update()
        {
            if (!AcceptsInput) { horizontalVelocity = Vector3.zero; IsSprinting = false; return; }
            var k = Keyboard.current;
            var input = k == null ? Vector2.zero : new Vector2(
                (k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0),
                (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0));
            bool held = k != null && (k.leftCtrlKey.isPressed || k.cKey.isPressed);
            IsCrouching = held || (IsCrouching && !CanStand());
            controller.height = Mathf.MoveTowards(controller.height, standingHeight * (IsCrouching ? crouchRatio : 1), 6 * Time.deltaTime);
            controller.center = Vector3.up * controller.height * .5f;
            head.localPosition = standingHeadPosition + Vector3.down * (standingHeight - controller.height);
            IsSprinting = k != null && k.leftShiftKey.isPressed && !IsCrouching && input.y > 0;
            float speed = IsCrouching ? walkSpeed * .5f : IsSprinting ? sprintSpeed : walkSpeed;
            Vector3 direction = transform.TransformDirection(new Vector3(input.x, 0, input.y));
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.ClampMagnitude(direction, 1) * speed, acceleration * Time.deltaTime);
            if (Grounded && verticalVelocity < 0) verticalVelocity = -2;
            if (k != null && k.spaceKey.wasPressedThisFrame && Grounded && !IsCrouching)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
            verticalVelocity = Mathf.Max(verticalVelocity + gravity * Time.deltaTime, -40);
            var flags = controller.Move((horizontalVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);
            if ((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0) verticalVelocity = 0;
        }

        bool CanStand()
        {
            float radius = controller.radius * .95f;
            Vector3 bottom = transform.position + Vector3.up * (controller.height - radius);
            Vector3 top = transform.position + Vector3.up * (standingHeight - radius);
            foreach (var hit in Physics.OverlapCapsule(bottom, top, radius, ~0, QueryTriggerInteraction.Ignore))
                if (!hit.transform.IsChildOf(transform)) return false;
            return true;
        }
    }
}
