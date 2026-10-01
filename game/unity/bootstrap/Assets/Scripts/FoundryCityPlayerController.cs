using UnityEngine;

namespace Dolzore
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class FoundryCityPlayerController : MonoBehaviour
    {
        public Camera playerCamera;
        public float walkSpeed = 5.2f;
        public float runSpeed = 8.0f;
        public float jumpHeight = 1.4f;
        public float gravity = -22f;
        public float mouseSensitivity = 2.0f;

        private CharacterController controller;
        private float verticalVelocity;
        private float pitch;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            HandleCursor();
            HandleLook();
            HandleMove();
        }

        private void HandleCursor()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void HandleLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked || playerCamera == null)
                return;

            float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
            float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(0f, mx, 0f);

            pitch = Mathf.Clamp(pitch - my, -80f, 80f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void HandleMove()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 wish = (transform.right * x + transform.forward * z);
            if (wish.sqrMagnitude > 1f)
                wish.Normalize();

            float speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                ? runSpeed
                : walkSpeed;

            if (controller.isGrounded)
            {
                if (verticalVelocity < 0f)
                    verticalVelocity = -2f;
                if (Input.GetKeyDown(KeyCode.Space))
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 velocity = wish * speed;
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);

            if (transform.position.y < -12f)
            {
                controller.enabled = false;
                transform.position = new Vector3(0f, 2.2f, -28f);
                verticalVelocity = 0f;
                controller.enabled = true;
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(16, 16, 365, 92), "DOLZORE — IRONWARD FOUNDRY CITY");
            GUI.Label(new Rect(30, 44, 340, 22), "WASD: move   Shift: run   Space: jump");
            GUI.Label(new Rect(30, 65, 340, 22), "Click: mouse look   Esc: release cursor");
            GUI.Label(new Rect(30, 86, 340, 22), "Explore the plaza, canal, foundry terraces and mine gate.");
        }
    }
}
