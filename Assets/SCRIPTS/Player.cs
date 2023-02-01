using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Jumping 
    public Animator anim;
    public CharacterController controller;
    public Transform cam, Gfx, groundcheck, vaultcheck;
    public LayerMask groundmask;
    Vector3 velocity;
    float jumpheight = 2f;
    float grounddistance = 0.4f;
    float gravity = -9.8f;
    float MoveSpeed, CurrentSpeed, SpeedVelocity, CurretVelocity;
    float SmoothRotTime = 0.1f;
    bool isGrounded;

    public FixedJoystick joystick;
    public FixedButton jumpButton, AimButton, TPS_Button, AttackButton;
    public bool aiming;

    void Start()
    {
        aiming = false;
        Gfx.position = transform.position;
    }

    void Update()
    {
        JumpAction();
        Aiming();
    }
    void FixedUpdate()
    {
        //////// //////////////////   PLAYER MOVEMENT AND ROTATION    ////////////////////
        Vector2 input;
        input = new Vector2(joystick.input.x, joystick.input.y);

        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            if (inputDir.magnitude >= 0.5f)
            {
                anim.SetFloat("mag", 1f);
                MoveSpeed = 3f;
            }
            else
            {
                anim.SetFloat("mag", 0f);
                MoveSpeed = 0.2f;
            }
            if (isGrounded)
            {
                if (joystick.input.x > 0f)
                    anim.SetFloat("mag", joystick.input.x);
                else if (joystick.input.y > 0f)
                    anim.SetFloat("mag", joystick.input.y);
            }
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref CurretVelocity, SmoothRotTime);

            float TargetSpeed = MoveSpeed * inputDir.magnitude;
            CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, TargetSpeed, ref SpeedVelocity, 0.1f);
            transform.Translate(transform.forward * CurrentSpeed * Time.deltaTime, Space.World);

        }
        else
            anim.SetFloat("mag", 0f);

    }

    public void JumpAction()
    {
        float range = 2.5f;
        RaycastHit HitInfo;
        bool canvault = false;
        bool vault = Physics.Raycast(vaultcheck.position, vaultcheck.forward, out HitInfo, range);
        isGrounded = Physics.CheckSphere(groundcheck.position, grounddistance, groundmask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (jumpButton.Pressed && isGrounded)
        {
            if (vault)
                canvault = HitInfo.transform.CompareTag("Jump Over");
            else
                canvault = false;
            if (canvault)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                anim.SetBool("vault", true);
            }
            else if (!canvault)
            {
                velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
                anim.SetBool("jump", true);
            }
        }
        else
        {
            anim.SetBool("jump", false);
            new WaitForSeconds(1f);
            anim.SetBool("vault", false);
            GetComponent<Rigidbody>().isKinematic = false;
        }
        Gfx.position = transform.position;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void Aiming()
    {
        if (AimButton.Pressed)
        {
            aiming = true;
            //CamDistance = 1f;

        }
        if (TPS_Button.Pressed)
        {
            aiming = false;
            //transform.eulerAngles = transform.eulerAngles - (new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 45f, transform.eulerAngles.z));
            //transform.SetParent(null);
            //CamDistance = 2.5f;
        }
    }
}
