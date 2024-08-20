using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBlock : BaseBlock
{
    public Cannon cannon = null;
    [SerializeField] TriggerEvents2D trigger;

    HashSet<Enemy> enemies = new HashSet<Enemy>();

    private void Start()
    {
        trigger.OnTriggerEnter += OnTriggerEnter2D;
        trigger.OnTriggerExit += OnTriggerExit2D;
    }

    void Update()
    {
        cannon.setTarget(CalculateClosestEnemy());
    }
    private GameObject CalculateClosestEnemy() 
    {
        float lowestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        foreach(Enemy enemy in enemies) 
        {
            float newContender = Vector3.Distance(this.transform.position, enemy.transform.position);
            if (newContender < lowestDistance) 
            {
                closestEnemy = enemy;
                lowestDistance = newContender;
            }
        }


        if (closestEnemy == null) 
        {
            return null;
        }

        return closestEnemy.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
}
