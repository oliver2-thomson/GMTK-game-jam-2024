using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParallax : MonoBehaviour
{
    public Transform Camera;
    [Space(10)]    
    public Transform[] BackgroundLayers;
    public float[] ScrollSpeeds;

    private void Update()
    {
        for (int i = 0;  i < BackgroundLayers.Length; i++)
        {
            float newLayerPos = Camera.position.x * ScrollSpeeds[i];
            BackgroundLayers[i].position = new Vector3(newLayerPos, Camera.position.y, 0);
        }
    }
}
