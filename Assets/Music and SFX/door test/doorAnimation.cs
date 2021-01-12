using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//may need to look into door animation again....here is my dot product scripted solution!!
public class doorAnimation : MonoBehaviour
{
    private bool doortoggle = false;

   //public Animator _animator = null;
    public GameObject player;
    private Quaternion currentRot;
    public bool dir;
    public float doorSpeed=120;
    //public Transform normal;

    void Start()
    {
        currentRot = transform.rotation;

    }
    void Update()
    {

        //transform.Translate(1, 0, 0);
       // transform.Rotate(0, 100*Time.deltaTime, 0);
       if (doortoggle )
        {
            float rotate;

            //dir has to do with if the X axis or the Z axis is normal to the player.... This got out of hand and I am still confused by it...
            //dot product allows the door to know which direction the player is coming from- see Multivariate/Linear Algerbra
            if (!dir)
            {
                
                rotate = Vector3.Dot(transform.forward, player.transform.forward) * -doorSpeed * Time.deltaTime + transform.rotation.eulerAngles.y;
            }
            else
            {
                rotate = Vector3.Dot(transform.right, player.transform.forward) * -doorSpeed * Time.deltaTime + transform.rotation.eulerAngles.y;
            }

          //stop the door from infinately turning.... disabled for now can be looked into
           //rotate = Mathf.Clamp(rotate, currentRot.y + 90, currentRot.y - 90);
          

            // dot product produces a scalar value so the float must be scaled by the Vector3 of (0,1,0)
            transform.rotation = Quaternion.Euler((rotate ) * Vector3.up);


           
        }
        else // this doesnt work but was supposed to act as a restorive force
        {
           // Debug.Log("close door now");
            Vector3 angleDiff = transform.rotation.eulerAngles - currentRot.eulerAngles;
            if (Mathf.Abs(angleDiff.magnitude) > 0)
            {
                Debug.Log(this.name + "close door now");

                transform.localRotation = Quaternion.Slerp(transform.localRotation, currentRot, angleDiff.magnitude);
            }
        }
       
    }

    //set global private toggle boolean to true so the door rotates
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == player)
        {
            Debug.Log(this.name + "door" + transform.forward + player.name + "playa" + player.transform.forward + "cos angle differnce " + Vector3.Dot(transform.forward, player.transform.forward));
            doortoggle = true;
            //_animator.SetBool("isopen", doortoggle);
            Debug.Log("OpenDoor");
        }

    }

    //set global private toggle boolean to false so the door rotates...but it doesnt so whatever
    void OnTriggerExit(Collider collider)
    {
        doortoggle = false;
        //_animator.SetBool("isopen", doortoggle);
        Debug.Log("CloseDoor");

        //updates currentRotaion variable.........THIS MAY BE WHY ITS NOT ROTATING BACKWARDS.....NOT SURE WHY I DID THIS
        currentRot = transform.rotation;
    }
    }