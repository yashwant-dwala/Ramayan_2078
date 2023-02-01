using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasButtonAnimations : MonoBehaviour
{
    public FixedButton jumpButton, AimButton, TpsButton, AttackButton, SpecialButton, HoldButton;
    [Space]
    public Animation XButton, LButton, RButton, SqButton, TriButton, OButton;
    [Space]
    public GameObject Aim;
    public Animator anim;



    private void Awake()
    {
        Aim.SetActive(false);
        XButton.Play();
        LButton.Play();
        RButton.Play();
        OButton.Play("O Default");
        TriButton.Play();
        SqButton.Play();
    }
    void Update()
    {
        Aim_And_Tps();
        Push_Pull_Action();
        JumpAction();
        Attack();
        RollAction();
        //FallDetect();
    }
    void Aim_And_Tps()
    {
        if (AimButton.Pressed)
        {
            Aim.SetActive(true);
            LButton.GetComponent<Animation>().GetClip("L Button");
            LButton.Play();
            anim.SetBool("aim", true);
        }
        else if (TpsButton.Pressed)
        {
            Aim.SetActive(false);
            RButton.GetComponent<Animation>().GetClip("R Button");
            RButton.Play();
            anim.SetBool("aim", false);
        }
    }
    void Attack()
    {
        if (AttackButton.Pressed)
        {
            SqButton.GetComponent<Animation>().GetClip("Sq Button");
            SqButton.Play();
        }
        if (AttackButton.Click)
        {
            anim.SetBool("attack", true);
            StartCoroutine("AttackComplete");
        }
    }
    void JumpAction()
    {
        if (jumpButton.Pressed)
        {
            XButton.GetComponent<Animation>().GetClip("X Button");
            XButton.Play();
        }
        if (jumpButton.Click)
        {
            anim.SetBool("jump", true);
            StartCoroutine(JumpComplete());
        }
    }
    void Push_Pull_Action()
    {
        if (HoldButton.Hold)
        {
            OButton.Play("O Pressed");
            anim.SetBool("hold", true);
        }  
        else if (HoldButton.Click)
        {
            OButton.Play("O Button");
            anim.SetBool("hold", false);
            new WaitForEndOfFrame();
        }
        else if (HoldButton.NoAction)
        {
            OButton.Play("O Default");
        }
    }
    void RollAction()
    {
        if (SpecialButton.Pressed)
        {
            TriButton.GetComponent<Animation>().GetClip("Tri Button");
            TriButton.Play();
        }
        if (SpecialButton.Click)
        {
            anim.SetBool("roll", true);
            StartCoroutine("RollComplete");
        }
    }
    /*void FallDetect()
    {
        if (!isGrounded)
            anim.SetBool("fall", true);
        else
            anim.SetBool("fall", false);
    }*/

    void SensitivetiveHoldDetect()
    {
        if (HoldButton.Pressed)
        {
            anim.SetBool("hold", true);                       //Sensetive Hold Detect  ( USE if NEEDED)
            StartCoroutine(HoldComplete());
        }
    }


    ///////////////////////////  COROUTINES   /////////////////////

    IEnumerator RollComplete()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("roll", false);
    }
    IEnumerator AttackComplete()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("attack", false);
    }
    IEnumerator JumpComplete()
    {
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("jump", false);
    }
    IEnumerator HoldComplete()
    {
        yield return new WaitForEndOfFrame();                 //Sensetive Hold Detect Coroutine
        anim.SetBool("hold", false);
    }
   

}