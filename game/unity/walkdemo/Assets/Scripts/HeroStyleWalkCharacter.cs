using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Dolzore
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class HeroStyleWalkCharacter : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 3.8f;
        public float turnSpeed = 12f;
        public float gravity = -22f;

        [Header("Visual rig")]
        public Transform visualRoot;
        public Transform body;
        public Transform head;
        public Transform leftArm;
        public Transform rightArm;
        public Transform leftLeg;
        public Transform rightLeg;
        public Transform hairTail;
        public Transform auraRingA;
        public Transform auraRingB;

        private CharacterController controller;
        private float verticalVelocity;
        private float gait;

        private Vector3 visualBasePosition;
        private Vector3 bodyBasePosition;
        private Vector3 bodyBaseScale;
        private Quaternion headBaseRotation;
        private Quaternion leftArmBaseRotation;
        private Quaternion rightArmBaseRotation;
        private Quaternion leftLegBaseRotation;
        private Quaternion rightLegBaseRotation;
        private Quaternion hairBaseRotation;
        private Vector3 ringABaseScale;
        private Vector3 ringBBaseScale;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            ResolveRigFallbacks();

            if (visualRoot != null) visualBasePosition = visualRoot.localPosition;
            if (body != null)
            {
                bodyBasePosition = body.localPosition;
                bodyBaseScale = body.localScale;
            }
            if (head != null) headBaseRotation = head.localRotation;
            if (leftArm != null) leftArmBaseRotation = leftArm.localRotation;
            if (rightArm != null) rightArmBaseRotation = rightArm.localRotation;
            if (leftLeg != null) leftLegBaseRotation = leftLeg.localRotation;
            if (rightLeg != null) rightLegBaseRotation = rightLeg.localRotation;
            if (hairTail != null) hairBaseRotation = hairTail.localRotation;
            if (auraRingA != null) ringABaseScale = auraRingA.localScale;
            if (auraRingB != null) ringBBaseScale = auraRingB.localScale;

            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            Vector2 input = ReadMovement();
            Vector3 planar = new Vector3(input.x, 0f, input.y);
            bool moving = planar.sqrMagnitude > 0.001f;

            if (moving)
            {
                planar.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(planar, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    1f - Mathf.Exp(-turnSpeed * Time.deltaTime));
            }

            if (controller.isGrounded)
                verticalVelocity = -2f;
            else
                verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = planar * moveSpeed;
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);

            AnimateVisuals(moving);
        }

        private void AnimateVisuals(bool moving)
        {
            float dt = Time.deltaTime;
            if (moving) gait += dt * 9.5f;
            else gait = Mathf.Lerp(gait, Mathf.Round(gait / Mathf.PI) * Mathf.PI, 1f - Mathf.Exp(-8f * dt));

            float wave = Mathf.Sin(gait);
            float absWave = Mathf.Abs(wave);
            float walkWeight = moving ? 1f : 0f;
            float idle = Mathf.Sin(Time.time * 2.25f);

            if (visualRoot != null)
            {
                float bob = moving ? absWave * 0.055f : idle * 0.018f;
                visualRoot.localPosition = visualBasePosition + Vector3.up * bob;
            }

            if (body != null)
            {
                body.localPosition = bodyBasePosition + Vector3.up * (moving ? absWave * 0.025f : idle * 0.01f);
                float breathe = 1f + idle * 0.008f;
                body.localScale = new Vector3(
                    bodyBaseScale.x * breathe,
                    bodyBaseScale.y * (1f + idle * 0.011f),
                    bodyBaseScale.z * breathe);
            }

            float legAngle = wave * 30f * walkWeight;
            float armAngle = -wave * 24f * walkWeight;

            if (leftLeg != null)
                leftLeg.localRotation = leftLegBaseRotation * Quaternion.Euler(legAngle, 0f, 0f);
            if (rightLeg != null)
                rightLeg.localRotation = rightLegBaseRotation * Quaternion.Euler(-legAngle, 0f, 0f);
            if (leftArm != null)
                leftArm.localRotation = leftArmBaseRotation * Quaternion.Euler(armAngle, 0f, -2f * walkWeight);
            if (rightArm != null)
                rightArm.localRotation = rightArmBaseRotation * Quaternion.Euler(-armAngle * 0.72f, 0f, 3f * walkWeight);

            if (head != null)
            {
                float nod = moving ? absWave * 2.2f : idle * 1.2f;
                head.localRotation = headBaseRotation * Quaternion.Euler(nod, 0f, 0f);
            }

            if (hairTail != null)
            {
                float sway = moving ? -wave * 9f : Mathf.Sin(Time.time * 1.7f) * 3f;
                hairTail.localRotation = hairBaseRotation * Quaternion.Euler(sway, 0f, sway * 0.35f);
            }

            if (auraRingA != null)
            {
                auraRingA.Rotate(0f, 26f * dt, 0f, Space.Self);
                float pulse = 1f + Mathf.Sin(Time.time * 3.1f) * 0.035f;
                auraRingA.localScale = ringABaseScale * pulse;
            }

            if (auraRingB != null)
            {
                auraRingB.Rotate(0f, -18f * dt, 0f, Space.Self);
                float pulse = 1f + Mathf.Sin(Time.time * 2.7f + 1.2f) * 0.04f;
                auraRingB.localScale = ringBBaseScale * pulse;
            }
        }

        private Vector2 ReadMovement()
        {
            float x = 0f;
            float y = 0f;

#if ENABLE_INPUT_SYSTEM
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) y += 1f;
#endif

            Vector2 result = new Vector2(x, y);
            return result.sqrMagnitude > 1f ? result.normalized : result;
        }

        private void ResolveRigFallbacks()
        {
            if (visualRoot == null) visualRoot = transform.Find("Visual");
            if (visualRoot == null) return;

            if (body == null) body = visualRoot.Find("Body");
            if (head == null) head = visualRoot.Find("HeadRig");
            if (leftArm == null) leftArm = visualRoot.Find("LeftArm");
            if (rightArm == null) rightArm = visualRoot.Find("RightArm");
            if (leftLeg == null) leftLeg = visualRoot.Find("LeftLeg");
            if (rightLeg == null) rightLeg = visualRoot.Find("RightLeg");
            if (hairTail == null) hairTail = visualRoot.Find("HeadRig/HairTail");
            if (auraRingA == null) auraRingA = visualRoot.Find("AuraRingA");
            if (auraRingB == null) auraRingB = visualRoot.Find("AuraRingB");
        }
    }
}
