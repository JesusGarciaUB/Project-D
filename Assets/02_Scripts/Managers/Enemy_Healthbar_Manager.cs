using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Healthbar_Manager : MonoBehaviour
{
    [SerializeField] private Slider healthbar;

    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void SetMax(int max)
    {
        healthbar.maxValue = max;
        healthbar.value = max;
    }

    public void SetValue(int val)
    {
        healthbar.value = val;
    }
}
