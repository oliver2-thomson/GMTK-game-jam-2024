using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{

    [SerializeField] private AudioSource audio;
    [SerializeField] private float maxVelocitySpeed;
    [SerializeField] private AnimationCurve velocityPitch;

    [SerializeField] private Transform directionTransform;
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

    private void Update()
    {
        if (audio != null)
        {
            audio.pitch = velocityPitch.Evaluate(rb.velocity.magnitude / maxVelocitySpeed);
        }

        if (directionTransform != null)
        {
            if (walkingLeft) 
                directionTransform.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                directionTransform.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
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
