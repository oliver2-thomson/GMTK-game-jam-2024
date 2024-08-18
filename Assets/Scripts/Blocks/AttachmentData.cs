using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttachmentData
{
    public AttachmentData(Vector2Int GridPosition, BaseBlock.FaceType Direction) 
    {
        direction = Direction;
        position = GridPosition;
    }

    public BaseBlock.FaceType direction;
    public Vector2Int position;
}
