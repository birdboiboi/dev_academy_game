using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initForce : MonoBehaviour
{
   
    public Rigidbody rg;

    //COOL RANDOM MOVEMENT SCRIPT TO SIMULATE INITIAL EXPLOSION added to rom b floaty objects
    void Start()
    {
        Debug.Log(rg.velocity);
        rg.GetComponent<Rigidbody>();
        rg.AddForce(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), ForceMode.Impulse);
       // rg.AddForce(1, 1, 1, ForceMode.Impulse);
    }

    
}
