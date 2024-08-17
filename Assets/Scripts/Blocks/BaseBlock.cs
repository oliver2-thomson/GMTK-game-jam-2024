using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    private bool[,] blockSlots = new bool[9, 9];
    
    public int BlockWidth
    {
        get 
        {
            return blockWidth;
        }
        set 
        {
            if (BlockWidth != value)
            {
                blockWidth = value;
                PopulateBlockSlots();
            }
        }
    }

    public int BlockHeight
    {
        get
        {
            return blockHeight;
        }
        set
        {
            if (blockHeight != value)
            {
                blockHeight = value;
                PopulateBlockSlots();
            }
        }
    }


    [HideInInspector] [SerializeField] private int blockWidth;
    [HideInInspector] [SerializeField] private int blockHeight;
    BaseBlock(int width, int height) 
    {
        blockWidth = width;
        blockHeight = height;

        blockSlots = new bool[blockWidth, blockHeight];
    }

    [ContextMenu("PopulateBlockSlots")]
    private void PopulateBlockSlots() 
    {
        blockSlots = new bool[blockWidth + 2, blockHeight + 2];
    }


    private bool GetLocalTileSlot(Vector2Int localPosition) 
    {
        return blockSlots[localPosition.x, localPosition.y];
    }
}
