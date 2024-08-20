using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseBlock : Damageable
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
    [Tooltip("Select what faces are attachable from this")]

    [SerializeField] private FaceType enumType;
    [SerializeField] private float DragSpeed;

    public bool AttachedToItem = false;
    [Space(10)]
    [Header("REQUIRED")]
    public RigidBodyCache rbCache;
    [SerializeField] private AudioClips DeathClip;

    [HideInInspector] public Transform DragSource;
    [HideInInspector] public AttachmentPoint CurrentAttPoint;
    [HideInInspector] public PlayerAttachment player;
    
    public override void Awake()
    {
        base.Awake();
        if (AttachedToItem)
        {
            //If attached at boot try and get player.
            //Can technically fail if attached to anything that isn't the player
            player = GetComponentInParent<PlayerAttachment>();
        }
        
    }

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

    public virtual void OnAttachBlock() 
    { 
        
    }
    public virtual void OnUseTile() 
    {
        
    }

    public virtual void OnToggleTile()
    {

    }

    [ContextMenu("Destroy Block")]
    public override void OnDeath() 
    {
        AudioSource.PlayClipAtPoint(DeathClip.GetRandomClip(), transform.position);
        player?.ForceDetachBlock(this);
        GameObject.Destroy(this.gameObject);
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
    }

    public bool CheckObjectIsntAttached(Transform transform) 
    {
        Rigidbody2D rb = transform.GetComponentInParent<Rigidbody2D>();
        return this.transform.IsChildOf(rb.transform) || rb.transform.IsChildOf(this.transform);
    }

    private void FixedUpdate()
    {
        if (DragSource != null)
        {
            rbCache.rb.velocity = (((Vector2)DragSource.transform.position - rbCache.rb.position) / 2) * DragSpeed;
        }
    }
}
