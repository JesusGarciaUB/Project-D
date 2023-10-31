using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 offset;
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - target.transform.position;
    }

    private void LateUpdate()
    {
        var dest = target.transform.position;
        dest.y = 2f;
        transform.position = Vector3.SmoothDamp(transform.position, dest, ref currentVelocity, smoothTime);
    }
}
