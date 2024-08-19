using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour
{
    public float Range;

    public Transform Target;

    bool Detected = false;

    Vector2 Direction;

    public GameObject Alarm;

    public GameObject Gun;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;

        Direction = targetPos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);
        Debug.Log(rayInfo.collider);




        if (rayInfo)
        {
            if(rayInfo.collider.gameObject.tag == "Player")
            {
                if(Detected == false)
                {
                    Detected = true;
                    Alarm.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            else
            {
                if (Detected == true)
                {
                    Detected = false;
                    Alarm.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        if(Detected)
        {
            Gun.transform.right = Direction;
            
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,Range);
        Gizmos.DrawRay(transform.position,Direction);
    }

    
}
