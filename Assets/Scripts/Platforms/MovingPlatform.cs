using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject target;
    public float speed = 50;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = target.transform.position - transform.position;
        if (displacement.magnitude < speed * Time.deltaTime)
        {
            target = target.GetComponent<PlatformPathNode>().getNext();
        }
        else
        {
            transform.position += displacement.normalized * speed * Time.deltaTime;
        }
    }
}
