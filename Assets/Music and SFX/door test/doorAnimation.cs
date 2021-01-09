using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimation : MonoBehaviour
{
    private bool doortoggle = false;

   //public Animator _animator = null;
    public GameObject player;
    private Quaternion currentRot;
    private bool dir;
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
           
            float rotate = Vector3.Dot(transform.right, player.transform.forward)   * -doorSpeed * Time.deltaTime + transform.rotation.eulerAngles.y;
            Debug.Log("urr3-->" + rotate);
           //rotate = Mathf.Clamp(rotate, currentRot.y + 90, currentRot.y - 90);
           // Debug.Log("urr4-->" + rotate);
            Debug.Log("urr rotate to" + (rotate) * Vector3.up);
            transform.rotation = Quaternion.Euler((rotate ) * Vector3.up);


            //transform.rotate(Vector3.Dot(transform.,0, player.transform.forward));
        }
        else
        {
           // Debug.Log("close door now");
            Vector3 angleDiff = transform.rotation.eulerAngles - currentRot.eulerAngles;
            if (Mathf.Abs(angleDiff.magnitude) > 0)
            {
                Debug.Log(this.name + "close door now");

                transform.rotation = Quaternion.Slerp(transform.rotation, currentRot, angleDiff.magnitude);
            }
        }
       
    }
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

    void OnTriggerExit(Collider collider)
    {
        doortoggle = false;
        //_animator.SetBool("isopen", doortoggle);
        Debug.Log("CloseDoor");

        currentRot = transform.rotation;
    }
    }