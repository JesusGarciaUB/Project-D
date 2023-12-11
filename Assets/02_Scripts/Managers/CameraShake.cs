using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 initialPosition;
    public float timeShake = 0;

    // Update is called once per frame
    void Update()
    {
        initialPosition = Level_Manager._LEVELMANAGER.player.transform.position;
        initialPosition.y += 2f;
        if (timeShake > 0)
        {
            timeShake -= Time.deltaTime;
            transform.position = initialPosition + Random.Range(-0.05f, 0.05f) * Vector3.right + Random.Range(-0.05f, 0.05f) * Vector3.up;
        }
        if (timeShake <= 0.01f && timeShake > 0)
        {
            transform.position = initialPosition;
            timeShake = 0;
        }
    }
}
