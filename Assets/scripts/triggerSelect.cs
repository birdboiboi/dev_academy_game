using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSelect : MonoBehaviour
{
    //ALL EVENT SCRIPTS NEED TO REFRENCE THIS BOOLEAN VARIABLE!!
    public bool isTriggered = false;

    //the object being checked for
    public GameObject key;

    //i tried making this glow but it didnt work...try adding new shaders
    public Shader shad;


    //when the collider's key is detected, set this boolean to true
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == key)
        {
            GetComponent<Renderer>().material.SetColor("_EMISSION", new Color(0.0927F, 0.4852F, 0.2416F, 0.42F));
            isTriggered = true;
            Destroy(key);
        }
        else
        {
            //enable this so that adding other object will set this to false(can be problimatic)
            //isTriggered = false;
        }
        
    }
    void OnTriggerExit(Collider col)
    {
        //if the key is removed this modifier is disabled
        if (col.gameObject == key)
        {
            isTriggered = false;
        }
    }

    }
