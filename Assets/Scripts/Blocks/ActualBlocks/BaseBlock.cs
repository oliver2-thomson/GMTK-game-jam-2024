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

    public enum ImpactMaterial
    {
        Metal = 0,
        Wood = 1,
        Dirt = 2,
        Blood = 3,
        Stone = 4
    }

    [EnumFlagsAttributes]
    [Tooltip("Select what faces are attachable from this")]

    [SerializeField] private FaceType enumType;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float DragSpeed;
    [SerializeField] private ImpactMaterial material; 

    public bool AttachedToItem = false;
    [Space(10)]
    [Header("REQUIRED")]
    public RigidBodyCache rbCache;

    [HideInInspector] public Transform DragSource;
    [HideInInspector] public AttachmentPoint CurrentAttPoint;
    [HideInInspector] public PlayerAttachment player;
    
    public float _Health 
    {
        get 
        {
            return currentHealth;
        }
        set 
        {
            if (value > MaxHealth)
            {
                currentHealth = MaxHealth;
            }
            else if (value < 0) 
            {
                currentHealth = 0;
                OnDeath();
            }
            else 
            {
                currentHealth = value;
            }
        }
    }

    private float currentHealth;

    private void Awake()
    {
        if (AttachedToItem)
        {
            //If attached at boot try and get player.
            //Can technically fail if attached to anything that isn't the player
            player = GetComponentInParent<PlayerAttachment>();
        }
        currentHealth = MaxHealth;
    }

    public void DamageAtPoint(Vector2 point, float damage) 
    {
        ImpactPropertiesManager.instance.PlayImpactPropertyAtPoint(material, point);
        DamageBlock(damage);
    }
    public void DamageBlock(float damage) 
    {
        _Health -= damage;
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
    public virtual void OnUseTile() 
    {
        
    }

    public virtual void OnToggleTile()
    {

    }

    [ContextMenu("Destroy Block")]
    public virtual void OnDeath() 
    {
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
        return this.transform.IsChildOf(rb.transform);
    }

    private void FixedUpdate()
    {
        if (DragSource != null)
        {
            rbCache.rb.velocity = (((Vector2)DragSource.transform.position - rbCache.rb.position) / 2) * DragSpeed;
        }
    }
}
