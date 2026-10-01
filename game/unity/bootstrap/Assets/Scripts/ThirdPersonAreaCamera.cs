using UnityEngine;

namespace Dolzore
{
    [DefaultExecutionOrder(100)]
    public sealed class ThirdPersonAreaCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(10f, 10f, -15f);
        [SerializeField] private float followSharpness = 5f;
        [SerializeField] private float lookHeight = 1.35f;

        public void Configure(Transform newTarget, Vector3 newOffset)
        {
            target = newTarget;
            offset = newOffset;
            if (target != null)
            {
                transform.position = target.position + offset;
                transform.rotation = Quaternion.LookRotation(
                    target.position + Vector3.up * lookHeight - transform.position,
                    Vector3.up);
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            float t = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, t);
            transform.rotation = Quaternion.LookRotation(
                target.position + Vector3.up * lookHeight - transform.position,
                Vector3.up);
        }
    }
}
