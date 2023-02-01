using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArrow : MonoBehaviour
{
    public float damage = 10f,range = 100f,firerate = 5f;
    public FixedButton AttackButton;
    public Camera fpsCam;
    public Transform arrowpos,trail,topPos,bottomPos,restPos;
    //public particlesystem muzzleflash;
    public GameObject arrowInstance,fakeAimArrow;
    public Player playerscript;  //public MobilePlayer movescript;
    Vector3 arrowmove,scaleToSet;
    public LineRenderer TopString,BottomString;

    private float nextTimetoFire = 0f;
    void Awake()
    {
        scaleToSet = trail.localScale;
        trail.localScale = Vector3.zero;
    }

    void FixedUpdate()
    {
        arrowmove -= Vector3.up*0.5f * Time.deltaTime;
      
    }
    void Update()
    {
        if (!playerscript.aiming)
            nextTimetoFire = Time.time + 1f / firerate;
        else if (AttackButton.Click && (Time.time >= nextTimetoFire))
        //if(AttackButton.Click)
        {
            nextTimetoFire = Time.time + 1f / firerate;  ////Grater the rate less is time ////
            Shoot();
        }            
        else if(AttackButton.Hold && !AttackButton.Click && AttackButton.Pressed)
        {
            new WaitForSeconds(.5f);
            trail.localScale = new Vector3(trail.localScale.x, 0f, trail.localScale.z);
            for (float i = 0; i <= scaleToSet.y; i += 0.2f)
            {
                 //new WaitForSeconds(.5f);
                 trail.localScale += new Vector3(0f, i, 0f);
                 trail.localScale = scaleToSet;
            }
        }
        if (!AttackButton.Hold)
        {
            trail.localScale = Vector3.zero;
        }
        if ((playerscript.aiming && AttackButton.Click) || !playerscript.aiming)
        {
            Vector3[] pos1 = { topPos.position, restPos.position };
            Vector3[] pos2 = { bottomPos.position, restPos.position };
            fakeAimArrow.SetActive(false);
            //TopString.SetPositions(pos1);
            //BottomString.SetPositions(pos2);
        }
        else
        {
            Vector3[] pos1 = { topPos.position, fakeAimArrow.transform.position};
            Vector3[] pos2 = { bottomPos.position, fakeAimArrow.transform.position};
            new WaitForSecondsRealtime(3f);
            fakeAimArrow.SetActive(true);
            //TopString.SetPositions(pos1);
            //BottomString.SetPositions(pos2);
        }
            
    }

    void Shoot()
    {
        //muzzleflash.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            new WaitForSecondsRealtime(1f);
            GameObject impactGO = Instantiate(arrowInstance,arrowpos.position + Vector3.up *.1f, arrowpos.rotation);
            new WaitForEndOfFrame(); 
            impactGO.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 600f, impactGO.transform.position);
            print(hit.transform.name);
            HitTarget target = hit.transform.GetComponent<HitTarget>();
            if(target!= null)
            {
                target.TakeDamage(damage);
            }
            if(hit.rigidbody!= null)
            {
                hit.rigidbody.AddForce(hit.normal* 90f);
            }
            //GameObject impactGO = Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,2f);
        }
    }
}
