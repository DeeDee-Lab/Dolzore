using UnityEngine;

namespace Dolzore
{
    public sealed class WalkDemoCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 5.4f, -8.2f);
        public float lookHeight = 1.7f;
        public float smooth = 8f;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            float t = 1f - Mathf.Exp(-smooth * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, t);

            Vector3 lookAt = target.position + Vector3.up * lookHeight;
            Quaternion targetRotation = Quaternion.LookRotation(lookAt - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        }
    }
}
