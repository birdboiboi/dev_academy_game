using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirror : MonoBehaviour
{
    public Camera cam;
    public Transform mirror2;
    public GameObject target;
    public bool switchStartingDirection = false;
    public bool onScreen = false;
    private bool oldOnScreen = false;
    private GameObject doppleganger;
    private CharMove targetScript;
    private float distanceToWall;
    public bool overideCheck = false;

    public float distCheck = 10;

    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.GetComponent<CharMove>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(target.transform.position);
        //overideCheck = (transform.position - target.transform.position).magnitude < distCheck;
        
        onScreen = (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && playerSeen());// || ( overideCheck)

        //Debug.Log(playerSeen() +"overall" + onScreen);
        if ( onScreen ) 
        {
            Debug.Log(playerSeen() + "overall" + onScreen);
            Vector3 distFromCamera = transform.position - target.transform.position;
            Debug.Log(this.transform.parent.name);
            if (onScreen != oldOnScreen)
            {
                // a ton of stuff to set up the dopple ganger ask Jordan for clarification.... watch for hierachy incase this gets an error
                targetScript.numReflection++;

                doppleganger = Instantiate(target, mirror2.localPosition + distFromCamera, transform.rotation);
                Destroy(doppleganger.GetComponent<CharMove>());
                Destroy(doppleganger.GetComponent<CharacterController>());
                doppleganger.transform.GetChild(1).GetComponent<Camera>().enabled = false;
                doppleganger.transform.GetChild(1).GetComponent<CameraHandle>().enabled = false;
                oldOnScreen = onScreen;
                doppleganger.transform.GetChild(1).GetComponent<AudioListener>().enabled = false;
                doppleganger.name = "doppleGanger";
                doppleganger.transform.GetChild(2).GetComponent<MonsterTrickle>().player = doppleganger;
                doppleganger.transform.GetChild(2).GetComponent<MonsterTrickle>().isDopple = true;

            }
            //Debug.Log(mirror2.rotation.eulerAngles);
            Quaternion yzSwitch = Quaternion.Euler(0, mirror2.rotation.eulerAngles.y- transform.rotation.eulerAngles.y, 0);
            //.Debug.Log(yzSwitch.eulerAngles);
            if (!switchStartingDirection)
            {
                distFromCamera.z = -distFromCamera.z;

            }
            else
            {

                distFromCamera.x = -distFromCamera.x;

            }
            distFromCamera.y = distFromCamera.y - Mathf.Abs(mirror2.transform.position.y - transform.position.y);

            doppleganger.transform.position = Vector3.Scale(mirror2.transform.position  + transformRotation(yzSwitch, distFromCamera) , new Vector3(1, 1, 1));// + new Vector3(1, target.transform.position.y, 1);
            doppleganger.transform.LookAt(mirror2);
           // doppleganger.transform.position = new Vector3(1, Mathf.Abs(target.transform.position.y - mirror2.transform.position.y), 1);//mirror2.transform.lossyScale.y / 2
            // Debug.Log("traget" + target.transform.GetChild(1).transform.childCount);
            //Debug.Log("Player" + doppleganger.transform.GetChild(1).transform.childCount);

            //if target camera does not have item AND  doppleganger camera  has item
            if (target.transform.GetChild(1).transform.childCount <= 0 && doppleganger.transform.GetChild(1).transform.childCount > 0)// && target.transform.GetChild(1).transform.childCount <= 0 )
            {
                Debug.Log("remove item");
                Destroy(doppleganger.transform.GetChild(1).transform.GetChild(0).gameObject);

            }
            //else if target's camera has item AND doppleganger camera does not
            else if (target.transform.GetChild(1).transform.childCount > 0 && doppleganger.transform.GetChild(1).transform.childCount <= 0)
            {
                Debug.Log("add item");
                Transform tempCam = target.transform.GetChild(1);
                Transform tempItem = target.transform.GetChild(1).transform.GetChild(0);
                Vector3 itemDistHold = tempCam.position - tempItem.position;
                Transform child = Instantiate(tempItem, doppleganger.transform.position + (doppleganger.transform.forward * itemDistHold.magnitude), doppleganger.transform.rotation);
                child.parent = doppleganger.transform.GetChild(1);

            }
            //distFromCamera.x = -distFromCamera.x;
            //doppleganger.transform.position = mirror2.transform.localPosition + (distFromCamera);// + mirror2.transform.TransformDirection(Vector3.up) ;
        }
        else
        {

            //only update the status of the player to have one less mirror see the player on the first time that the player is out of view
            if (oldOnScreen != onScreen)
            {
                targetScript.numReflection -= 1;
                oldOnScreen = onScreen;
                //clears doppleganger
                Destroy(doppleganger);
                //reset the monster's position on exiting the mirrors view
                targetScript.ResetThismonster();
            }

        }



    }
    /*
        void OnTriggerEnter(Collider col)
        {
            Debug.Log("enter");
                if (col.gameObject == target)
                {
                    col.gameObject.transform.position = mirror2.transform.position;
                }
        }
        */

    //linear agerbra over here!!! applys rotational matrix
    Vector3 transformRotation(Quaternion rotation, Vector3 coordinates)
    {
        Matrix4x4 m = Matrix4x4.Rotate(rotation);
        return m.MultiplyPoint3x4(coordinates);
    }

    //cast a ray towards the player, ifthere is a wall in the way the player is considered not seen
    bool playerSeen()
    {
        //may be redundant...or may break the camera checking for human script
        bool isSeenBool = false;

        //get the directional vector of the player's location vs this mirror's
        Vector3 dirVect = -Vector3.Normalize(transform.position - target.transform.position);

        Debug.Log(this.name + "(overideCheck && distanceToWall < distCheck)" + (Mathf.Abs((transform.position - target.transform.position).magnitude)));
        RaycastHit hit;

        //create a ray cast object from this point and propogate till infinity checking for intersection
        Ray distWall = new Ray(transform.position, dirVect);
        Debug.DrawRay(cam.WorldToViewportPoint(target.transform.position), dirVect * 100, Color.yellow);
        if (Physics.Raycast(distWall, out hit))
        {

            
            distanceToWall = hit.distance;
            //Debug.DrawRay(cam.WorldToViewportPoint(target.transform.position), dirVect * hit.distance, Color.yellow);
            //Debug.Log(hit.collider.gameObject.name + distanceToWall);
            //Debug.DrawRay(transform.position, dirVect * distanceToWall, Color.yellow);


            //did the player hit the target?
            isSeenBool = (hit.collider.gameObject == target);


        }        //Debug.Log("none");

        //some stuff about overide...I think I fixed this by changing how far the camera can look
        Debug.Log(this.name + (false || (overideCheck && true)));
        return (isSeenBool || (overideCheck && distanceToWall < distCheck));

        // Debug.Log(this.name + distanceToWall); Mathf.Abs((transform.position - target.transform.position).magnitude)< distCheck

    }
}