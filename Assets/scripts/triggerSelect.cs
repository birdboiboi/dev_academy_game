using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSelect : MonoBehaviour
{
    public bool isTriggered = false;
    public GameObject key;
    public Shader shad;
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
            GetComponent<Renderer>().material.SetColor("_EMISSION", new Color(0.0927F, 0.4852F, 0.2416F, 0.42F));
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
