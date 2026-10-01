using UnityEngine;

namespace Dolzore
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class ThirdPersonWalker3D : MonoBehaviour
    {
        public float moveSpeed=6.0f;
        public float runSpeed=9.0f;
        public float turnSpeed=12f;
        public float jumpHeight=1.5f;
        public float gravity=-22f;
        private CharacterController cc;
        private float vertical;

        private void Awake(){ cc=GetComponent<CharacterController>(); }

        private void Update()
        {
            var cam=Camera.main;
            Vector3 f=cam?cam.transform.forward:Vector3.forward;
            Vector3 r=cam?cam.transform.right:Vector3.right;
            f.y=0; r.y=0; f.Normalize(); r.Normalize();

            float h=Input.GetAxisRaw("Horizontal"), v=Input.GetAxisRaw("Vertical");
            Vector3 dir=(f*v+r*h);
            if(dir.sqrMagnitude>1f)dir.Normalize();
            float speed=(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))?runSpeed:moveSpeed;

            if(cc.isGrounded){
                vertical=-1.5f;
                if(Input.GetButtonDown("Jump")) vertical=Mathf.Sqrt(jumpHeight*-2f*gravity);
            } else vertical+=gravity*Time.deltaTime;

            Vector3 motion=dir*speed; motion.y=vertical;
            cc.Move(motion*Time.deltaTime);

            if(dir.sqrMagnitude>0.01f){
                var target=Quaternion.LookRotation(dir,Vector3.up);
                transform.rotation=Quaternion.Slerp(transform.rotation,target,turnSpeed*Time.deltaTime);
            }
        }
    }
}
