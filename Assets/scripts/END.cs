using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class END : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("end" + collider.gameObject.name);
        
            Application.Quit();
        
    }
}
