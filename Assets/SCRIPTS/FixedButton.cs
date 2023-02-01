using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    FixedButton Btn;
    float press_time, reqHoldTime = 0.7f;
    public bool Click, Hold, NoAction;
    public bool Pressed;

    // Use this for initialization
    void Start()
    {
        Click = false;
        Hold = false;
        NoAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        HoldCheck();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        Click = false;  // Check for Error
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        Click = false;
    }
    public void HoldCheck()
    {
        if (Pressed)
        {
            press_time += Time.deltaTime;
            if (press_time >= reqHoldTime)
            {
                Hold = true;
                Click = false;
                NoAction = false;
            }
            else if (0.7f >= press_time && press_time > 0)
            {
                Click = true;
                Hold = false;
                NoAction = false;
            }
            else
            {
                NoAction = true;
                Hold = false;
                Click = false;
            }
                
        }
        // Reset Hold Time
        if (!Pressed)
        {
            press_time = 0;
            Hold = false;
            Click = false;
            NoAction = true;
        }

    }
}
