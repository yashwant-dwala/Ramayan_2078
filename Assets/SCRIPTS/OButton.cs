using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OButton : MonoBehaviour
{
    FixedButton Btn;
    float press_time, reqHoldTime = 0.2f;
    bool Click,Hold,NoAction;


    private void Start()
    {
        Btn = gameObject.GetComponent<FixedButton>();
        Click = false;
        Hold = false;
        NoAction = true;
    }

    public void HoldCheck()
    {
        if (Btn.Pressed)
        {
            press_time += Time.deltaTime;
            if (press_time >= reqHoldTime)
            {
                Hold = true;
            }
            else if (0.2f >= press_time && press_time > 0)
                Click = true;
            else
                NoAction = true;
        }
        // Reset Hold Time
        if (!Btn.Pressed)
        {
            press_time = 0;
            Hold = false;
            Click = false;
        }
            
    }
    void Update()
    {
        HoldCheck();
        if (Hold)
            print("Hold");
        else if (Click)
            print("click");
        else if(NoAction)
            print("none");
    }
}
