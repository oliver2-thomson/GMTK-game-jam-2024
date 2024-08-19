using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Special camera movements are handled here, when the player walks into a camera-locked area
    public static CameraController instance;
    private void Awake()
    {
        instance = this;    
    }

    public Transform Target;
    [Space(10)]
    public CameraLock CurrentCameraLock;

    private void Update()
    {
        if (CurrentCameraLock != null)
        {
            transform.position = new Vector2(
                CurrentCameraLock.CameraLockX ? CurrentCameraLock.transform.position.x : Target.position.x,
                CurrentCameraLock.CameraLockY ? CurrentCameraLock.transform.position.y : Target.position.y);
        }
        else
        {
            transform.position = Target.position;
        }
    }
}
