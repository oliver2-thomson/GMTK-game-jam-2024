using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneActivator : MonoBehaviour
{
    // Activates all objects in the area if the player enters it
    public List<GameObject> ObjectsToActive = new List<GameObject>();

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ActivateAllObjects()
    {
        foreach(GameObject obj in ObjectsToActive)
        {
            // We can use a switch here for special objects if needed
            obj.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateAllObjects();
        }
    }
}
