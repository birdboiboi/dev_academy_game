using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundRandom : MonoBehaviour
{

    public AudioClip[] sounds;
    public AudioSource ad;
    private float timeNext = 0;


    //BJ this is a random noise(literal hearing noise) picker
    //every random seconds between 0 to 90 a noise will be played
    void Update()
    {

        float timeCurrent = Time.time;
        if (timeCurrent > timeNext)
        {
            ad.clip = sounds[Random.Range(0, sounds.Length - 1)];
            ad.Play();
            timeNext = timeCurrent + Random.Range(0, 90);
            Debug.Log("tip");
        }
    }
}
