using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundRandom : MonoBehaviour
{

    public AudioClip[] sounds;
    public AudioSource ad;
    private float timeNext = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
