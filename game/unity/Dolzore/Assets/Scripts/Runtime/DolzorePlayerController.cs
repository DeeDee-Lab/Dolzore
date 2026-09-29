using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dolzore
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public sealed class DolzorePlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4.25f;
        private Rigidbody2D body;
        private Vector2 movement;

        public event Action<Vector2> Moved;
        public event Action InteractPressed;

        public Vector2 WorldPosition => transform.position;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Update()
        {
            movement = ReadMovement();
            if (movement.sqrMagnitude > 1f) movement.Normalize();

            if (WasInteractPressed())
                InteractPressed?.Invoke();
        }

        private void FixedUpdate()
        {
            body.linearVelocity = movement * moveSpeed;
            Moved?.Invoke(body.position);
        }

        private static Vector2 ReadMovement()
        {
            var value = Vector2.zero;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) value.x -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) value.x += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) value.y -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) value.y += 1f;
            }

            if (Gamepad.current != null)
            {
                var stick = Gamepad.current.leftStick.ReadValue();
                var dpad = Gamepad.current.dpad.ReadValue();
                value += stick.sqrMagnitude > 0.04f ? stick : dpad;
            }
            return Vector2.ClampMagnitude(value, 1f);
        }

        private static bool WasInteractPressed()
        {
            return (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
                || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
        }
    }
}
