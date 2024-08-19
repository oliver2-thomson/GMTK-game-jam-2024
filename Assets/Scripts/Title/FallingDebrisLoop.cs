using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebrisLoop : MonoBehaviour
{
    public float Speed;
    public float LoopPointY;

    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0));

        if (transform.position.y <= LoopPointY)
        {
            transform.Translate(new Vector3(0, col.size.y * 2, 0));
        }
    }
}
