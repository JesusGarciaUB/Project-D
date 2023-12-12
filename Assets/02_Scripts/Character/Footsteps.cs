using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;

    public void PlayFootStep()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = Random.Range(0.9f, 1.0f);
        audio.pitch = Random.Range(0.8f, 1.2f);

        audio.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }
}
