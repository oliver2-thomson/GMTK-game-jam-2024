using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseBlock baseblock = collision.GetComponentInParent<BaseBlock>();

        baseblock.DamageAtPoint(collision.ClosestPoint(transform.position));
    }
}
