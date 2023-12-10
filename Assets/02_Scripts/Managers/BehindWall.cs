using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindWall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float alpha;
    private float ogAlpha;
    private Color ogColor;
    private Color newColor;
    private Material material;
    public bool playerBehind = false;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        ogAlpha = material.color.a;
    }

    private void NewAlpha(float newAlpha)
    {
        ogColor = material.color;
        newColor = new Color(ogColor.r, ogColor.g, ogColor.b, Mathf.Lerp(ogColor.a, newAlpha, speed * Time.deltaTime));
        material.color = newColor;
    }

    private void Update()
    {
        if (playerBehind) NewAlpha(alpha);
        else NewAlpha(ogAlpha);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerBehind = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerBehind = false;
        }
    }
}
