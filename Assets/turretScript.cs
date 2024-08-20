using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class turretScript : MonoBehaviour
{
    public float Range;

    public Transform Target;

    bool Detected = false;
    Vector2 Direction;

    public GameObject Alarm;
    public GameObject Gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;

        // Perform the raycast
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);
        Debug.Log(rayInfo.collider);
        if(rayInfo.collider != null && rayInfo.collider.GetComponentInParent<PlayerController>() != null)
        {
            if (!Detected)
            {
                Detected = true;
                Alarm.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            if (Detected)
            {
                Detected = false;
                Alarm.GetComponent<SpriteRenderer>().color = Color.green;
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
        Gizmos.DrawRay(transform.position, Direction);
    }
}
