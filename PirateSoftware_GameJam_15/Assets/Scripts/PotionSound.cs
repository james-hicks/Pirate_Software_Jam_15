using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSound : MonoBehaviour
{
    public static PotionSound instance;

    public List<AudioClip> sounds = new List<AudioClip>();

    private void Start()
    {
        instance = this;
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().clip = sounds[Random.Range(0, sounds.Count)];
        GetComponent<AudioSource>().Play();
    }
}
