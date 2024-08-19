using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBlock : BaseBlock
{
    public Cannon cannon = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            cannon.setTarget(enemy.gameObject);
        }
    }
}
