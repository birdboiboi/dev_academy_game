using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSelect : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == key)
        {
            isTriggered = true;
            Destroy(key);
        }
        else
        {
            isTriggered = false;
        }
        
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == key)
        {
            isTriggered = false;
        }
    }

    }
