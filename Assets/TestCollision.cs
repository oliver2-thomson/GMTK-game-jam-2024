using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] private float damage = 0.8f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseBlock baseblock = collision.GetComponentInParent<BaseBlock>();

        baseblock.DamageAtPoint(collision.ClosestPoint(transform.position), damage);
    }
}
