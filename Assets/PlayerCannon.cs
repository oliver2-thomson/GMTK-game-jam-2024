using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : BaseBlock
{
    bool isToggled = false;
    public override void OnToggleTile()
    {
        isToggled = !isToggled;
    }

}
