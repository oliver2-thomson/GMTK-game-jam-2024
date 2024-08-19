using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainBlock : BaseBlock
{
    public Vector2Int localGridPosition;

    public override void OnDeath()
    {
        Debug.Log("OH NO PLAYER DIED!");
    }
}
