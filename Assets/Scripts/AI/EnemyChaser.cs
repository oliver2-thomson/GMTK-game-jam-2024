using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{
    public override void OnDetect(PlayerAttachment player)
    {
        IgnoreRationality = true;
        _player = player;
    }

    public override void OnLosePlayer(PlayerAttachment player)
    {
        IgnoreRationality = false;
        _player = null;
    }

    public override void UpdateDetect()
    {
        float offset = _player.transform.position.x - transform.position.x;

        if (offset > 0) 
        {
            walkingLeft = false;
        }
        else 
        {
            walkingLeft = true;
        }


        rb.AddForce(direction * speed * Time.deltaTime);
    }
}
