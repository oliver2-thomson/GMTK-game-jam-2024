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
    [SerializeField] private ImpactMaterial material; 

    public bool AttachedToItem = false;
    [Space(10)]
    [Header("REQUIRED")]
    public RigidBodyCache rbCache;

    [HideInInspector] public Transform DragSource;
    [HideInInspector] public AttachmentPoint CurrentAttPoint;

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
            else 
            {
                currentHealth = value;
            }
        }
    }

    private float currentHealth;


    public void DamageAtPoint(Vector2 point) 
    {
        ImpactPropertiesManager.instance.PlayImpactPropertyAtPoint(material, point);
    }
    public void DamageBlock() 
    {
        
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
    }

    private void FixedUpdate()
    {
        if (DragSource != null)
        {
            rbCache.rb.position = DragSource.transform.position;
        }
    }
}
