using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event01 : MonoBehaviour
{
    public triggerSelect trigger;
    //public Material mat;
    //public Material mat2;
    //private Renderer rend;
    public float exampleVal=1;
    // Start is called before the first frame update
   
    
    // Update is called once per frame
    void Update()
    {

        //this is an example of refrencing the triggerSelect Script @SYDNEY TAKE A LOOK AT THIS!!!!
        if (trigger.isTriggered == true)
        {
            //move this object up and down
            transform.position = new Vector3(0, Mathf.Sin(Time.time)* exampleVal, 0)  + transform.position;
        }
        else
        {
            //??? forgot why I didnt just leave it as  transform.position = transform.position
            transform.position =new Vector3(0, 0, 0)+ transform.position;
        }
    }
    
}
