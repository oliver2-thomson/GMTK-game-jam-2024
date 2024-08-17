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
    [SerializeField] private FaceType enumType;
    public bool AttachedToItem = false;
    [Space(10)]
    public Transform DragSource;
    public AttachmentPoint CurrentAttPoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public void TurnOnRigidbody()
    {
        rb.isKinematic = false;
    }

    public void TurnOffRigidbody()
    {
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (DragSource != null)
        {
            rb.position = DragSource.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out AttachmentPoint newPoint))
        {
            CurrentAttPoint = newPoint;
        }
    }
}
