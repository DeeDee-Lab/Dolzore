using UnityEngine;

namespace Dolzore
{
    public sealed class TownCameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector2 minBounds = new Vector2(-16f, -10f);
        public Vector2 maxBounds = new Vector2(16f, 10f);
        public float smoothTime = 0.14f;

        private Vector3 velocity;

        private void LateUpdate()
        {
            if (target == null) return;

            float x = Mathf.Clamp(target.position.x, minBounds.x, maxBounds.x);
            float y = Mathf.Clamp(target.position.y, minBounds.y, maxBounds.y);
            Vector3 desired = new Vector3(x, y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
