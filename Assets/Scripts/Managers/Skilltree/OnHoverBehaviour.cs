using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnHoverBehaviour : MonoBehaviour
{
    private GameObject textBox;
    private string text;

    private void Awake()
    {
        textBox = Level_Manager._LEVELMANAGER.onHoverObject;
    }
    public void MouseEnter()
    {
        textBox.SetActive(true);
        textBox.GetComponent<HoverObject>().SetPosition(transform.position + new Vector3(0, 50 + (float)(textBox.GetComponent<RectTransform>().rect.height * 0.5)));
        textBox.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void MouseExit()
    {
        textBox.SetActive(false);
    }

    public void SetText(string val) { this.text = val; }
}
