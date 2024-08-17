using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachment : MonoBehaviour
{

    BaseBlock[,] BlockList = new BaseBlock[10,10];
    bool[,] blockAttachmentPoints = new bool[10, 10];
    

    void AttachBlock(Vector2Int localPosition) 
    {
        if (CheckPositionIfValid(localPosition)) 
        {
            
        }
    }

    bool CheckPositionIfValid(Vector2Int position) 
    {
        if (BlockList[position.x, position.y] != null) 
        {
            return false;
        }

        if (blockAttachmentPoints[position.x, position.y]) 
        {
            return true;
        }

        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
