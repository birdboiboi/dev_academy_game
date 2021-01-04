using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initForce : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rg;
    void Start()
    {
        Debug.Log(rg.velocity);
        rg.GetComponent<Rigidbody>();
        rg.AddForce(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), ForceMode.Impulse);
       // rg.AddForce(1, 1, 1, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
