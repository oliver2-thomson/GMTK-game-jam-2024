using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : BaseBlock
{
    public override void OnToggleTile(bool isToggled)
    {
        isToggled = !isToggled;
        base.OnToggleTile(isToggled);
    }

}
