using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beyblade : MonoBehaviour
{
    public float speed = 5;
    private Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        rotation = new Vector3(0, 0, speed);
        transform.eulerAngles += rotation * Time.deltaTime;
    }
}
