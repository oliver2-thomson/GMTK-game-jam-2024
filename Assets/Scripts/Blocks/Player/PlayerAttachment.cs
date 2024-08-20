using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerAttachment : MonoBehaviour
{

    BaseBlock[,] BlockList = new BaseBlock[20,20];
    List<AttachmentData> attachmentData = new List<AttachmentData>();
    //bool[,] blockAttachmentPoints = new bool[10, 10];

    [Header("REQUIRED")]
    [SerializeField] private GameObject highlight_prefab;
    [SerializeField] private Transform highlightParent;
    [SerializeField] private BrainBlock brain;

    [Header("Tile Positional Data")]
    [SerializeField] Vector2 tileOffset = new Vector2(1,1);
    [Tooltip("Defines where the midpoint of the character starts")]
    [SerializeField] int middleOffset = 5;
    [SerializeField] public Transform tileParent;

    [Header("Input Settings")]
    [SerializeField] public KeyCode ToggleBlockActionKey;
    [SerializeField] public KeyCode UseBlockActionKey;

    List<AttachmentPoint> allHighlightObjects = new List<AttachmentPoint>();

    private PlayerController playerController;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartBackend();
        StartVisuals();
    }

    public void AttachBlock(Vector2Int localPos, BaseBlock block) 
    {
        if (CheckPositionIfValid(localPos)) 
        {
            //Attach block to player
            block.transform.parent = tileParent;
            block.transform.localPosition = ConvertFromLocalBlockToLocalPosition(localPos);
            

            //Update Internal Tables
            BlockList[localPos.x, localPos.y] = block;
            AddAttachmentsPointsFromBlock(localPos, block);
            block.AttachedToItem = true;

            //Add Mass to main character
            rb.mass += block.rbCache.rb.mass;
            rb.drag += block.rbCache.rb.drag;
            rb.angularDrag += block.rbCache.rb.angularDrag;

            block.rbCache.DeleteOldRigidbody();
            block.player = this;
            block.OnAttachBlock();

            //Recalculate Attachment Points
            ShowAttachmentUI();

            //Move player's legs to bottom part of the block "stack"
            playerController.FindBottomMostPoint();
        }
    }

    private void ActuallyDetachBlock(Vector2Int blockPosition)
    {
        BaseBlock block = BlockList[blockPosition.x, blockPosition.y];

        List<AttachmentData> blockFaces = GetAllFacesFromAttachedBlock(new Vector2Int(blockPosition.x, blockPosition.y));
        foreach (AttachmentData face in blockFaces)
        {
            RemoveFaceFromAttachments(face);
        }

        block.rbCache.RecreateRigidbody();
        block.AttachedToItem = false;
        block.transform.parent = null;
        block.player = null;

        rb.mass -= block.rbCache.rb.mass;
        rb.drag -= block.rbCache.rb.drag;
        rb.angularDrag -= block.rbCache.rb.angularDrag;


        BlockList[blockPosition.x, blockPosition.y] = null;

        //Recalculate Attachment Points
        ShowAttachmentUI();

        //Move player's legs to bottom part of the block "stack"
        playerController.FindBottomMostPoint();
    }
    

    public void ForceDetachBlock(BaseBlock block)
    {
        for (int i = 0; i < BlockList.GetLength(0); i++)
        {
            for (int j = 0; j < BlockList.GetLength(1); j++)
            {
                if (BlockList[i, j] == block)
                {
                    HashSet<Vector2Int> blocksEffected;
                    if (WillRemovingThisLeaveSurroundingBlocksOnTheirOwn(new Vector2Int(i, j), out blocksEffected))
                    {
                        foreach (Vector2Int blockPosition in blocksEffected)
                        {
                            ActuallyDetachBlock(blockPosition);
                        }
                    }

                    //Detatch Core
                    ActuallyDetachBlock(new Vector2Int(i, j));
                }
            }
        }
    }

    public bool TryDetachBlock(BaseBlock block)
    {
        bool detached = false;
        for (int i = 0; i < BlockList.GetLength(0); i++)
        {
            for (int j = 0; j < BlockList.GetLength(1); j++)
            {
                if (BlockList[i, j] == block && WillRemovingThisLeaveSurroundingBlocksOnTheirOwn(new Vector2Int(i, j)))
                {
                    ActuallyDetachBlock(new Vector2Int(i, j));
                    detached = true;
                }
            }
        }

        return detached;
    }

    void RemoveFaceFromAttachments(AttachmentData face) 
    {
        HashSet<AttachmentData> garbageCollection = new HashSet<AttachmentData>();
        foreach (AttachmentData attachment in attachmentData)
        {
            if (attachment.position == face.position && attachment.direction == face.direction)
            {
                garbageCollection.Add(attachment);
            }
        }

        //Taking out the trash!
        foreach (AttachmentData garbage in garbageCollection) 
        {
            attachmentData.Remove(garbage);
        }
        
    }

    List<AttachmentData> GetAllFacesFromAttachedBlock(Vector2Int localPos) 
    {
       BaseBlock block = BlockList[localPos.x, localPos.y];
       List<AttachmentData> currentFaces = new List<AttachmentData>();


       List<int> faceIDs = block.ReturnAllFaceElements();
        foreach (BaseBlock.FaceType Id in faceIDs)
        {
            Vector2Int offset;
            switch (Id)
            {
                case BaseBlock.FaceType.Top:
                    offset = localPos + new Vector2Int(0, 1);
                    break;
                case BaseBlock.FaceType.Right:
                    offset = localPos + new Vector2Int(1, 0);
                    break;
                case BaseBlock.FaceType.Left:
                    offset = localPos + new Vector2Int(-1, 0);
                    break;
                case BaseBlock.FaceType.Bottom:
                    offset = localPos + new Vector2Int(0, -1);
                    break;
                default:
                    offset = new Vector2Int(0, 0);
                    Debug.LogError("Invalid Face");
                    break;
            }
            if (!CheckBlockExists(offset))
                currentFaces.Add(new AttachmentData(offset, Id));
        }

        return currentFaces;
    }


    void AddAttachmentsPointsFromBlock(Vector2Int localPos, BaseBlock block)
    {
        List<int> FaceIDs = block.ReturnAllFaceElements();

        foreach(int Id in FaceIDs) 
        {
            switch (Id) 
            {
                case ((int)BaseBlock.FaceType.Top):
                    AddSingleAttachmentPoint(localPos + new Vector2Int(0, 1), (BaseBlock.FaceType)Id);
                    break;
                case ((int)BaseBlock.FaceType.Right):
                    AddSingleAttachmentPoint(localPos + new Vector2Int(1, 0), (BaseBlock.FaceType)Id);
                    break;
                case ((int)BaseBlock.FaceType.Left):
                    AddSingleAttachmentPoint(localPos + new Vector2Int(-1, 0), (BaseBlock.FaceType)Id);
                    break;
                case ((int)BaseBlock.FaceType.Bottom):
                    AddSingleAttachmentPoint(localPos + new Vector2Int(0, -1), (BaseBlock.FaceType)Id);
                    break;

            }
                
        }
    }

    void AddSingleAttachmentPoint(Vector2Int offsetPoint, BaseBlock.FaceType direction) 
    {
        if (!CheckBlockExists(offsetPoint)) 
        {
            attachmentData.Add(new AttachmentData(offsetPoint, direction));
        }
    }

    bool CheckBlockExists(Vector2Int pos) 
    {
        if (pos.x < 0 || pos.x > BlockList.GetLength(0) - 1) 
        {
            return true;
        }
        else if (pos.y < 0 || pos.y > BlockList.GetLength(0) - 1) 
        {
            return true;
        }

        return BlockList[pos.x, pos.y] != null;
    }

    bool CheckPositionIfValid(Vector2Int position) 
    {
        if (BlockList[position.x, position.y] != null) 
        {
            return false;
        }

        
        if (attachmentData.Exists(s => s.position == position)) 
        {
            return true;
        }

        return false;
    }

    private void StartBackend() 
    {
        //Set Brain Spot
        BlockList[10, 10] = brain;
        brain.localGridPosition = new Vector2Int(10, 10);

        //Set all sides for default attachments
        AddAttachmentsPointsFromBlock(new Vector2Int(10, 10), brain);
    }

    HashSet<Vector2Int> GetAllActiveAttachmentPoints()
    {
        HashSet<Vector2Int> validAttachmentPoints = new HashSet<Vector2Int>();

        foreach(AttachmentData data in attachmentData) 
        {
            if (!CheckBlockExists(data.position)) 
            {
                validAttachmentPoints.Add(data.position);
            }
        }
        

        return validAttachmentPoints;
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
        return new Vector3((localPos.x - middleOffset) * tileOffset.x, (localPos.y - middleOffset) * tileOffset.y);
    }

    [ContextMenu("HideAttachments")]
    public void HideUI()
    {
        foreach (AttachmentPoint UI in allHighlightObjects)
        {
            UI.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(ToggleBlockActionKey)) 
        {
            ToggleAllBlocks();
        }
        if (Input.GetKeyDown(UseBlockActionKey)) 
        {
            UseAllBlocks();
        }
    }

    private void ToggleAllBlocks() 
    {
        foreach(BaseBlock block in BlockList) 
        {
            if (block != null) 
            {
                block.OnToggleTile();
            }
        }
    }

    private void UseAllBlocks() 
    {
        foreach (BaseBlock block in BlockList)
        {
            if (block != null)
            {
                block.OnUseTile();
            }
        }
    }
}
