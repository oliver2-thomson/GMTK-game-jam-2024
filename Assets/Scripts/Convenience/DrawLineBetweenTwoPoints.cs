using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLineBetweenTwoPoints : MonoBehaviour
{
    [SerializeField] bool UpdatePerFrame = true;
    [SerializeField] private Transform[] positions;

    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        UpdateLineRendererPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdatePerFrame) 
        {
            UpdateLineRendererPosition();
        }    
    }

    void UpdateLineRendererPosition() 
    {
        List<Vector3> convertedPos = new List<Vector3>();
        foreach(Transform trans in positions) 
        {
            convertedPos.Add(trans.position);
        }

        line.SetPositions(convertedPos.ToArray());
    }
}
