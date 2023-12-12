using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayOnSpawn : MonoBehaviour
{
    private AudioSource audioS;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
        audioS.Play();
        Destroy(gameObject, audioS.clip.length);
    }
}
