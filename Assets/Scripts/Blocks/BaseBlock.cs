using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    [System.Flags]
    public enum FaceType
    {
        Top = 0, 
        Right = 1, 
        Bottom = 2, 
        Left = 3
    }

    [EnumFlagsAttributes]
    private FaceType enumType;


    public List<int> ReturnAllFaceElements()
    {
        List<int> selectedElements = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(FaceType)).Length; i++)
        {
            int layer = 1 << i;
            if (((int)enumType & layer) != 0)
            {
                selectedElements.Add(i);
            }
        }

        return selectedElements;
    }

    public void OnPlacedTile() 
    {
        //Todo parse oriational data to block
    }


    /// <summary>
    /// Use this to Check what face is an attachable point on a block
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    public bool CheckFace(FaceType face) 
    {
        List<int> faceList = ReturnAllFaceElements();

        return faceList.Contains((int)face);
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
