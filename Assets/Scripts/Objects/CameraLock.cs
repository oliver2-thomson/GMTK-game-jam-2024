using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    public bool CameraLockX;
    public bool CameraLockY;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
