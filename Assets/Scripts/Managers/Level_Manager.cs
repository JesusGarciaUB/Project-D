using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    //aqui guardamos al jugador en escena para que las demas clases puedan acceder a el
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject onHoverObject;

    public static Level_Manager _LEVELMANAGER;

    private void Awake()
    {
        _LEVELMANAGER = this;
        Physics.queriesHitTriggers = true;
    }

    public Vector3 GetPlayerPositionGround0()
    {
        return new Vector3(player.transform.position.x, 0, player.transform.position.z);
    }
}
