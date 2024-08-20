using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainBlock : BaseBlock
{
    public Vector2Int localGridPosition;

    private UIGameOver gameOver;

    public override void Awake()
    {
        base.Awake();
        gameOver = FindObjectOfType<UIGameOver>();
    }

    public override void OnDeath()
    {
        gameOver.OnGameOver();
    }
}
