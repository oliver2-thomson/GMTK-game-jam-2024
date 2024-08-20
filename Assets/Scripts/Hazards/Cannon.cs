using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject target = null;
    public GameObject projectilePrefab;
    public float projectilePower;
    [SerializeField] private float damage;
    public float fireRate = 2;
    private float cooldownProgress = 0;

    // Update is called once per frame
    void Update()
    {
        if (cooldownProgress < fireRate)
        {
            cooldownProgress += Time.deltaTime;
        }

        if (target != null)
        {
            Vector3 displacement = target.transform.position - transform.position;
            float xDelta = displacement.x;
            float yDelta = displacement.y;

            float angle = Mathf.Atan2(yDelta, xDelta) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);

            if (cooldownProgress >= fireRate)
            {
                cooldownProgress -= fireRate;
                Fire();
            }
        }
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (target != null)
        {
            Vector3 displacement = target.transform.position - transform.position;
            Projectile newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            rb.AddForce(displacement.normalized * projectilePower);
            newProjectile.GimmeDamage(damage, this.transform.GetComponentInParent<Rigidbody2D>().transform);
        }
    }

    public void setTarget (GameObject newTarget)
    {
        target = newTarget;
    }
}
