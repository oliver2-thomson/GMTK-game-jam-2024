using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider2D))]
public class Zone : MonoBehaviour
{

#if UNITY_EDITOR
    [Header("Gizmos")]
    public Color gizmoColor = Color.white;
#endif

    [Header("Zone Data")]

    [SerializeField] private Zone[] LinkedZones;

    [Space]

    [SerializeField] private ZoneItem[] zoneItems;


    [Space]

    [Header("Unity Events")]
    [Space]

    [SerializeField] UnityEvent OnPrimaryEnter;
    [SerializeField] UnityEvent OnSecondaryEnter;
    [SerializeField] UnityEvent OnPrimaryOneShotEnter;
    [SerializeField] UnityEvent OnPrimaryExit;

    private bool hasOneShot = false;

    private void Awake()
    {
        BoxCollider[] boxes = GetComponents<BoxCollider>();
        
        foreach (BoxCollider box in boxes) 
        {
            box.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            OnZoneEnter();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            OnZoneExit();
        }
    }

    private void OnZoneEnter()
    {
        OnPrimaryEnter?.Invoke();
        if (!hasOneShot) 
        {
            hasOneShot = true;
            OnPrimaryOneShotEnter?.Invoke();
        }

        //Primary Items
        InvokeZoneItems(ZoneItem.ZoneType.Primary);

        //Secondary Items
        foreach(Zone zone in LinkedZones) 
        {
            zone.OnSecondaryEnter?.Invoke();
            zone.InvokeZoneItems(ZoneItem.ZoneType.Secondary);
        }
    }

    private void InvokeZoneItems(ZoneItem.ZoneType zoneType, bool OnEnter = true) 
    {
        foreach(ZoneItem item in zoneItems) 
        {
            if (item.CheckZone(zoneType)) 
            {
                if (OnEnter)
                    item.OnZoneEnter();
                else
                    item.OnZoneExit();
            }
        }
    }
    private void OnZoneExit() 
    {
        OnPrimaryExit?.Invoke();

        //Primary Items
        InvokeZoneItems(ZoneItem.ZoneType.Primary, false);

        //Secondary Items
        foreach (Zone zone in LinkedZones)
        {
            zone.InvokeZoneItems(ZoneItem.ZoneType.Secondary, false);
        }

    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.5f);

        BoxCollider2D collider;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        style.alignment = TextAnchor.MiddleCenter;

        if (TryGetComponent(out collider))
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
            Gizmos.color = new Color(0, 0, 0, 0);
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size / 5);

            Handles.Label(collider.bounds.center, gameObject.name, style);

        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.5f);

        if (TryGetComponent<BoxCollider2D>(out var collider))
        {
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);

            foreach (Zone zone in LinkedZones)
            {
                Gizmos.color = zone.gizmoColor;
                Gizmos.DrawLine(collider.bounds.center, zone.GetComponent<Collider2D>().bounds.center);
            }

        }

    }
#endif
}
