using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Base_Button
{
    public override void MouseClick()
    {
        Application.Quit();
    }
}
