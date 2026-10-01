using UnityEngine;

namespace Dolzore
{
    public sealed class ThirdPersonCamera3D : MonoBehaviour
    {
        public Transform target;
        public float distance=8.5f;
        public float height=3.6f;
        public float sensitivity=2.2f;
        public float smooth=10f;
        private float yaw=0f;
        private float pitch=18f;

        private void Start(){ if(target!=null) yaw=target.eulerAngles.y; }
        private void LateUpdate()
        {
            if(target==null)return;
            if(Input.GetMouseButton(1) || Cursor.lockState==CursorLockMode.Locked){
                yaw+=Input.GetAxis("Mouse X")*sensitivity;
                pitch-=Input.GetAxis("Mouse Y")*sensitivity;
                pitch=Mathf.Clamp(pitch,-5f,55f);
            }
            if(Input.GetKeyDown(KeyCode.Tab)) Cursor.lockState=Cursor.lockState==CursorLockMode.Locked?CursorLockMode.None:CursorLockMode.Locked;

            Quaternion rot=Quaternion.Euler(pitch,yaw,0);
            Vector3 desired=target.position + Vector3.up*height - rot*Vector3.forward*distance;
            transform.position=Vector3.Lerp(transform.position,desired,1f-Mathf.Exp(-smooth*Time.deltaTime));
            transform.LookAt(target.position+Vector3.up*1.6f);
        }
    }
}
