using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    private float damage = 10;
    private Vector3 rotation = new Vector3(0, 0, 20);

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other.GetComponent<BaseBlock>());
        }
    }
}
