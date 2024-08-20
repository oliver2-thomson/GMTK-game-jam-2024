using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSpawner : ZoneItem
{
    [Space]
    [Header("Drag enemy from Prefab/AI to Below!")]
    [SerializeField] Enemy enemy;
    [SerializeField] bool isWalkingLeft = false;
    [Space]
    [SerializeField] UnityEngine.Events.UnityEvent onDeath;

    bool hasSpawned = false;
    public override void OnZoneEnter()
    {
        if (!hasSpawned) 
        {
            Enemy spawned_enemy = GameObject.Instantiate(enemy, transform.position, transform.rotation, null).GetComponent<Enemy>();
            spawned_enemy._OnDeath += OnDeath;
            spawned_enemy.walkingLeft = isWalkingLeft;
            hasSpawned = true;
        }
    }

    public void OnDeath() 
    {
        onDeath?.Invoke();
    }

    public void OnDrawGizmos()
    {
        if (enemy != null)
        {
            Gizmos.DrawIcon(transform.position, enemy.gameObject.name);
        }
    }
}
