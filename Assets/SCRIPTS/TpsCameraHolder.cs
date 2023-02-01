using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsCameraHolder : MonoBehaviour
{
    
    public FixedTouchField TouchField;
    public FixedButton AimButton,TpsButton;
    public Transform Gfx, CameraLookTarget, AimPos,NormalPos,spine,spinelookrot;
    public float SmoothRotTime = 0.1f;
    public float CamRotMinVert = -40f, CamRotMaxVert = 40f;
    public bool aiming = false;
    float CurretVelocity;
    float SpeedVelocity;
    float CurrentSpeed;

    //FOR SCREEN SETTINGS
    public float CamDistance = 2.5f;
    public float RotationSensitivity = 0.2f;
    float xAxis, yAxis, SmoothTime = 0.12f;
    Vector3 CurrentVel, targetRotation;


    void Update()
    {
        Aiming();
        if (aiming)
        {
            spine.rotation.SetLookRotation(spinelookrot.position);
            spine.rotation.SetFromToRotation(spine.position, spinelookrot.position);
        }
    }

    void LateUpdate()
    {
        //  Camera follow script in late update to fix damping

        xAxis -= TouchField.TouchDist.y * RotationSensitivity;
        yAxis += TouchField.TouchDist.x * RotationSensitivity;

        xAxis = Mathf.Clamp(xAxis, CamRotMinVert, CamRotMaxVert);
        //yAxis = Mathf.Clamp(yAxis, CamRotMinHor, CamRotMaxHor);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis, 0f), ref CurrentVel, SmoothTime);
        NormalPos.eulerAngles = targetRotation;
        NormalPos.position = CameraLookTarget.position - transform.forward * CamDistance;

    }
    void Aiming()
    {
        if (AimButton.Pressed)
        {
            aiming = true;
            transform.SetPositionAndRotation(AimPos.position, AimPos.rotation);
        }
        else if(TpsButton.Pressed)
        {
            aiming = false;
            transform.SetPositionAndRotation(NormalPos.position, NormalPos.rotation);
        }           
    }
}
