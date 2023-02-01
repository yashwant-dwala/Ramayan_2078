using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push_Pull_Objects : MonoBehaviour
{

    public Transform player;
    public Transform cam;
    public FixedButton HoldButton;
    float throwforce = 9000f;
    bool isCarried = false, hasPlayer =false;
    private bool touched = false;
  
    private void LateUpdate()
    {
        OnTriggerEnter();
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2.5f)
            hasPlayer = true;
        else
            hasPlayer = false;
        if(hasPlayer && HoldButton.Hold)
        {
            //GetComponent<Rigidbody>().isKinematic = true;
            // transform.position = Vector3.up * (-1.706f);
            //InvokeRepeating("PosCorrect", 0.5f, 600f);
            transform.parent = player;
            Invoke("PosCorrect", 0.5f);
            isCarried = true;
        }
        if (isCarried)
        {
            if (HoldButton.Click)
            {
                ///GetComponent<Rigidbody>().isKinematic = false;
                isCarried = false;
                transform.parent = null;
                GetComponent<Rigidbody>().AddForce(cam.forward * throwforce);
            }
        }
        void OnTriggerEnter()
        {
            if (isCarried)
                touched = true;
        }
        void PosCorrect()
        {
            transform.position = Vector3.zero;
        }
    }

}
