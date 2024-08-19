using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoneItem : MonoBehaviour
{
    [System.Flags]
    public enum ZoneType
    {
        Primary,
        Secondary
    }

    [EnumFlagsAttributes]
    public ZoneType ZoneTrigger;


    public abstract void OnZoneEnter();

    public virtual void OnZoneExit() 
    {
        
    }

    public List<int> ReturnAllFaceElements()
    {
        List<int> selectedElements = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(ZoneType)).Length; i++)
        {
            int layer = 1 << i;
            if (((int)ZoneTrigger & layer) != 0)
            {
                selectedElements.Add(i);
            }
        }

        return selectedElements;
    }

    public bool CheckZone(ZoneType face)
    {
        List<int> faceList = ReturnAllFaceElements();

        return faceList.Contains((int)face);
    }

}
