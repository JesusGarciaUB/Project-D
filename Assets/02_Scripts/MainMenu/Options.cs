using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : Base_Button
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject options;

    public override void MouseClick()
    {
        textmesh.color = ogColor;
        options.SetActive(true);
        parent.SetActive(false);
    }
}
