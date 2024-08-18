using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = target.transform.position - transform.position;
        float xDelta = displacement.x;
        float yDelta = displacement.y;

        float angle = Mathf.Atan2(yDelta, xDelta) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
