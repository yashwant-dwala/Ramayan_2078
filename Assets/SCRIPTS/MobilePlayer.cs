using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlayer : MonoBehaviour
{
    //FOR SCREEN SETTINGS
    public Transform CameraLookTarget;
    public GameObject Cam, AimCam;
    Transform cam,aimcam;
    public float CamDistance = 2.5f;
    public float RotationSensitivity = 8f;
    float xAxis, yAxis, SmoothTime = 0.12f;
    Vector3 targetRotation, CurrentVel;

    [Space]

    //FOR CAMERA + PLAYER SETTINGS
    public Transform Gfx;
    public float SmoothRotTime = 0.2f;
    public float MoveSpeed = 0.1f;
    float CurretVelocity;
    float SpeedVelocity;
    float CurrentSpeed;
    public float CamRotMinVert = -40f, CamRotMaxVert = 40f;

    [Space]

    //FOR JUMP ACTION
    public Animator anim;
    public CharacterController controller;
    public Transform groundcheck,vaultcheck;
    public LayerMask groundmask;
    Vector3 velocity;
    float jumpheight = 2f;
    float grounddistance = 0.4f;
    float gravity = -9.8f;
    bool isGrounded;

    [Space]

    // ANDROID SETUP
    public FixedJoystick joystick;
    public FixedTouchField TouchField;
    public FixedButton jumpButton;
    //public Animation XButton,LButton;
    public bool AndroidControls = false;

    [Space]

    //AIMING ACTION
    public FixedButton AimButton, TPS_Button, AttackButton;
    //public GameObject fakeAimArrow;
    public bool aiming;

    void Start()
    {
        cam = Cam.transform;
        aimcam = AimCam.transform;
        CamDistance = 2.5f;
        if (AndroidControls)
        {
            RotationSensitivity = 0.2f;
            SmoothRotTime = 0.1f;
        }
        aiming = false;
        Cam.SetActive(true);
        AimCam.SetActive(false);
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
        if (AndroidControls)
        {
            input = new Vector2(joystick.input.x, joystick.input.y);
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

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
    void LateUpdate()
    {
        //  Camera follow script in late update

        if (AndroidControls)
        {
            xAxis -= TouchField.TouchDist.y * RotationSensitivity;
            yAxis += TouchField.TouchDist.x * RotationSensitivity;
        }
        else
        {
            xAxis -= Input.GetAxis("Mouse Y") * RotationSensitivity;
            yAxis += Input.GetAxis("Mouse X") * RotationSensitivity;
        }
        xAxis = Mathf.Clamp(xAxis, CamRotMinVert, CamRotMaxVert);
        //yAxis = Mathf.Clamp(yAxis, CamRotMinHor, CamRotMaxHor);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis, 0f), ref CurrentVel, SmoothTime);
        cam.eulerAngles = targetRotation;
        cam.position = CameraLookTarget.position - cam.forward * CamDistance;

        if (aiming) {
            //aimcam.SetParent(this.transform);
            //transform.SetParent(AimCam.transform);
            transform.eulerAngles  = targetRotation ;
            //aimcam.eulerAngles = targetRotation;
            //aimcam.position = CameraLookTarget.position + new Vector3(0.1f, 0.15f, -0.34f);
        }

    }

    public void JumpAction()
    {
        float range = 2.5f;
        RaycastHit HitInfo;
        bool canvault = false;
        bool vault = Physics.Raycast(vaultcheck.position,vaultcheck.forward,out HitInfo,range);
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
            CamDistance = 1f;
            Cam.SetActive(false);
            AimCam.SetActive(true);
        }
        if (TPS_Button.Pressed)
        {
            aiming = false;
            transform.eulerAngles = transform.eulerAngles - (new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 45f, transform.eulerAngles.z));
            transform.SetParent(null);
            CamDistance = 2.5f;
            Cam.SetActive(true);
            AimCam.SetActive(false);
        }
    }
    /////////////////////      COROUTINES         //////////////////////////

}


