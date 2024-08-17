using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachment : MonoBehaviour
{

    BaseBlock[,] BlockList = new BaseBlock[10,10];
    bool[,] blockAttachmentPoints = new bool[10, 10];

    [Header("REQUIRED")]
    [SerializeField] private GameObject highlight_prefab;
    [SerializeField] private Transform highlightParent;
    [SerializeField] private GameObject debugTile;

    [Header("Tile Positional Data")]
    [SerializeField] Vector2 tileOffset = new Vector2(1,1);
    [Tooltip("Defines where the midpoint of the character starts")]
    [SerializeField] int middleOffset = 5;
    [SerializeField] Transform tileParent;

    List<AttachmentPoint> allHighlightObjects = new List<AttachmentPoint>();


    void AttachBlock(Vector2Int localPos, BaseBlock block) 
    {
        if (CheckPositionIfValid(localPos)) 
        {
            //Attach block to player
            block.transform.parent = tileParent;
            block.transform.localPosition = ConvertFromLocalBlockToLocalPosition(localPos);

            //Update Internal Tables
            BlockList[localPos.x, localPos.y] = block;
            blockAttachmentPoints[localPos.x, localPos.y] = false;
            AddAttachmentsPointsFromBlock(localPos, block);

            HideUI();
        }
    }
    
    void AddAttachmentsPointsFromBlock(Vector2Int localPos, BaseBlock block)
    {
        List<int> FaceIDs = block.ReturnAllFaceElements();

        foreach(int Id in FaceIDs) 
        {
            switch (Id) 
            {
                case ((int)BaseBlock.FaceType.Top):
                    Debug.Log("Top");
                    break;
                case ((int)BaseBlock.FaceType.Right):
                    Debug.Log("Right");
                    break;
                case ((int)BaseBlock.FaceType.Left):
                    Debug.Log("left");
                    break;
                case ((int)BaseBlock.FaceType.Bottom):
                    Debug.Log("bottom");
                    break;

            }
                
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
        //Set all sides for default attachments
        blockAttachmentPoints[4, 5] = true;
        blockAttachmentPoints[5, 4] = true;
        blockAttachmentPoints[6, 5] = true;
        blockAttachmentPoints[5, 6] = true;
    }

    HashSet<Vector2Int> GetAllActiveAttachmentPoints()
    {
        HashSet<Vector2Int> attachmentPoints = new HashSet<Vector2Int>();

        for (int x = 0; x < blockAttachmentPoints.GetLength(0); x++) 
        {
            for (int y = 0; y < blockAttachmentPoints.GetLength(1); y++) 
            {
                if (blockAttachmentPoints[x, y]) 
                {
                    attachmentPoints.Add(new Vector2Int(x, y));    
                }
            }
        }

        return attachmentPoints;
    }

    [ContextMenu("ShowAttachments")]
    public void ShowAttachmentUI()
    {
        List<Vector2Int> attachPoints = new List<Vector2Int>(GetAllActiveAttachmentPoints());
        
        int difference = attachPoints.Count - allHighlightObjects.Count;

        //Add to UI cache if not big enough.
        for (int i = difference; i > 0; i--) 
        {
            allHighlightObjects.Add(GameObject.Instantiate(highlight_prefab, highlightParent).GetComponent<AttachmentPoint>());
        }

        foreach(AttachmentPoint UI in allHighlightObjects) 
        {
            UI.gameObject.SetActive(false);
        }

        for(int i = 0; i < attachPoints.Count; i++)
        {
            allHighlightObjects[i].gameObject.SetActive(true);
            allHighlightObjects[i].attachPoint = attachPoints[i];
            allHighlightObjects[i].transform.localPosition = ConvertFromLocalBlockToLocalPosition(attachPoints[i]);
        }

    }

    private Vector3 ConvertFromLocalBlockToLocalPosition(Vector2Int localPos) 
    {
        return new Vector3(localPos.x - middleOffset * tileOffset.x, localPos.y - middleOffset * tileOffset.y);
    }

    [ContextMenu("HideAttachments")]
    public void HideUI()
    {
        foreach (AttachmentPoint UI in allHighlightObjects)
        {
            UI.gameObject.SetActive(false);
        }
    }


    [ContextMenu("TestAttachment")]
    void TestAttachment()
    {
        AttachBlock(allHighlightObjects[0].attachPoint, GameObject.Instantiate(debugTile).GetComponent<BaseBlock>());
    }
}
