using UnityEngine;

namespace Dolzore
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Prototype25DPlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4.2f;
        [SerializeField] private Transform visualRoot;

        private CharacterController controller;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (visualRoot == null && transform.childCount > 0)
                visualRoot = transform.GetChild(0);
        }

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 move = new Vector3(x, 0f, z);
            if (move.sqrMagnitude > 1f) move.Normalize();

            controller.SimpleMove(move * moveSpeed);

            if (move.sqrMagnitude > 0.001f && visualRoot != null)
            {
                Quaternion target = Quaternion.LookRotation(move, Vector3.up);
                visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, target, 14f * Time.deltaTime);
            }
        }
    }
}
