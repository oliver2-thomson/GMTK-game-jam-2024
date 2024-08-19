using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSpawner : ZoneItem
{
    [Space]
    [Header("Drag enemy from Prefab/AI to Below!")]
    [SerializeField] Enemy enemy;

    bool hasSpawned = false;
    public override void OnZoneEnter()
    {
        if (!hasSpawned) 
        {
            GameObject.Instantiate(enemy, transform.position, transform.rotation, null);
            hasSpawned = true;
        }
    }

    public void OnDrawGizmos()
    {
        if (enemy != null)
        {
            Gizmos.DrawIcon(transform.position, enemy.gameObject.name);
        }
    }
}
