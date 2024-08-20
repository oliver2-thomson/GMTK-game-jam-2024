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

    public GameObject CannonBall;
    public float FireRate;
    public float TimeTofire = 0;
    [SerializeField] private float damage = 1;

    public Transform TargetPoint;

    public float Force;

    // Start is called before the first frame update
    void Start()
    {
        Target = FindObjectOfType<PlayerAttachment>().transform;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;

        // Perform the raycast
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);
        if (rayInfo.collider != null && rayInfo.collider.GetComponentInParent<PlayerController>() != null)
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

        if (Detected)
        {
            Gun.transform.right = Direction;
            if (Time.time > TimeTofire)
            {
                TimeTofire = Time.time + 1 / FireRate;
                shoot();
            }
        }
    }
    void shoot()
    {
        GameObject CannonBallIns = Instantiate(CannonBall, TargetPoint.position, Quaternion.identity);
        CannonBallIns.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * Force);
        CannonBallIns.GetComponent<Projectile>().GimmeDamage(damage, this.transform);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
        Gizmos.DrawRay(transform.position, Direction);
    }



}



