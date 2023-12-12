using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base_Button : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI textmesh;
    protected Color ogColor;

    private void Awake()
    {
        ogColor = textmesh.color;
    }

    public void MouseEnter()
    {
        textmesh.color = Color.grey;
    }

    public void MouseExit()
    {
        textmesh.color = ogColor;
    }

    public virtual void MouseClick() { }
}
