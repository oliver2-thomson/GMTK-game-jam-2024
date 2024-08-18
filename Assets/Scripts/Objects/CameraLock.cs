using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : ZoneItem
{
    public bool CameraLockX;
    public bool CameraLockY;

    public override void OnZoneEnter()
    {
        CameraController.instance.CurrentCameraLock = this;
    }

    public override void OnZoneExit()
    {
        CameraController.instance.CurrentCameraLock = null;
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
