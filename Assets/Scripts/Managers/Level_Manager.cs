using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] public GameObject player;

    public static Level_Manager _LEVELMANAGER;

    private void Awake()
    {
        _LEVELMANAGER = this;
    }
}
