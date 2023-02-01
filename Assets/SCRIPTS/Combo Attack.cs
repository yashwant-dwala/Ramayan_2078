using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    public Animator HoldBtnAnim;
    bool Hold_Pressed;
    int Hold_Pressed_State;
    public void HoldBtnPressed()
    {
        if (Hold_Pressed_State == 0)
        {
            HoldBtnAnim.Play("O default");
            Hold_Pressed_State = 1;
            return;
        }
        if (Hold_Pressed_State != 0)
        {
            if (Hold_Pressed)
            {
                Hold_Pressed = false;
                Hold_Pressed_State += 1;
            }
        }
    }
    public void Hold_Can_Be_Pressed()
    {
        Hold_Pressed = true;
    }
    public void Hold()
    {
        if (Hold_Pressed_State == 1)
            HoldBtnAnim.Play("O Button");
        if (Hold_Pressed_State == 2)
            HoldBtnAnim.Play("O Pressed");
    }
    public void HoldReset()
    {
        Hold_Pressed = false;
        Hold_Pressed_State = 0;

    }
}
