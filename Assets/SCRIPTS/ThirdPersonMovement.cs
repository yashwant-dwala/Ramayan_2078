using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public float speed=20f;
    public float turnsmoothtime = 0.1f;
    public float gravity = -9.8f;
    float turnsmoothvelocity;
    public Transform cam;


    public Vector3 velocity;
    public float jumpheight=6f;
    public Transform groundcheck;
    public float grounddistance=0.4f;
    public LayerMask groundmask;
    bool isGrounded;
    public bool mobile = false;
    //public FixedJoystick joystick;
    
    
    void FixedUpdate()
    {
        MoveAction();
        JumpAction();
    }
    public void MoveAction(){
       
        float Hor = Input.GetAxisRaw("Horizontal");
        float Vert = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(Hor,0f,Vert).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (Input.GetMouseButton(1))
                anim.SetBool("run", true);
            else
                anim.SetBool("run", false);
            if (isGrounded) { 
                anim.SetFloat("mag", direction.magnitude);
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle + 90, ref turnsmoothvelocity, turnsmoothtime);
            transform.rotation = Quaternion.Euler(0f, Angle, 0f);

            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(movDir.normalized * speed * Time.deltaTime);
        }
        else
            anim.SetFloat("mag", 0f);
    }

    void JumpAction(){
        isGrounded =Physics.CheckSphere(groundcheck.position,grounddistance,groundmask);

        if(isGrounded && velocity.y < 0){
            velocity.y =-2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
            anim.SetBool("jump", true);
        }
        else { 
            anim.SetBool("jump", false);
        }
            velocity.y += gravity*Time.deltaTime;
            controller.Move(velocity*Time.deltaTime);
    }

}
