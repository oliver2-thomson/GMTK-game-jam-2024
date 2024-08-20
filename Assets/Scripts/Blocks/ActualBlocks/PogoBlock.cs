using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogoBlock : BaseBlock
{
    [SerializeField] private Pogo pogo;
     
    public override void OnUseTile()
    {
        pogo.UsePogo();
    }
}
