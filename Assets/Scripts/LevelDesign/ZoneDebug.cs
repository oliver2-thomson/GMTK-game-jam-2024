using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDebug : ZoneItem
{
    public override void OnZoneEnter()
    {
        Debug.Log("Hello!");
    }

    public override void OnZoneExit()
    {
        
    }
}
