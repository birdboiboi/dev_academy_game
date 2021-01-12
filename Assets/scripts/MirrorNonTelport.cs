using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorNonTelport : MonoBehaviour
{

    public Camera cam;
    public Transform mirror2;
    public GameObject target;
    public bool onScreen = false;
    private bool oldOnScreen = false;
    private CharMove targetScript;
    private float distanceToWall;

    // Start is called before the first frame update
    void Start()
    {
        targetScript = target.GetComponent<CharMove>();
    }

    // Update is called once per frame
    void Update()
    {

        //get the camera center screen 2d point converted to a 3d location
        Vector3 screenPoint = cam.WorldToViewportPoint(target.transform.position);
        Debug.Log("player is seen" + playerSeen());
        //check if the player is within the convertedbounding view of the camera and invoke the player seen function 
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && playerSeen();

        Debug.Log("player is screen" + onScreen);
        if (onScreen)
        {
            //only update the player's status once on the first time the mirror sees the player
            if (onScreen != oldOnScreen)
            {
                targetScript.numReflection++;
                oldOnScreen = onScreen;
            }

        }
        else
        {
            //only update the status of the player to have one less mirror see the player on the first time that the player is out of view
            if (oldOnScreen != onScreen)
            {
                targetScript.numReflection -= 1;
                oldOnScreen = onScreen;
                //reset the monster's position on exiting the mirrors view
                targetScript.ResetThismonster();

            }

        }
    }


    //cast a ray towards the player, ifthere is a wall in the way the player is considered not seen
        bool playerSeen()
        {
        //get the directional vector of the player's location vs this mirror's
            Vector3 dirVect = -Vector3.Normalize(transform.position - target.transform.position);

        //create a ray cast object from this point and propogate till infinity checking for intersection
            RaycastHit hit;
            Ray distWall = new Ray(transform.position, dirVect);
            if (Physics.Raycast(distWall, out hit))
            {
                
               // Debug.DrawRay(cam.WorldToViewportPoint(target.transform.position), dirVect * hit.distance, Color.yellow);
               // Debug.Log(hit.collider.gameObject.name + distanceToWall);
                //Debug.DrawRay(transform.position, dirVect * distanceToWall, Color.yellow);

                //did the player hit the target?
                return (hit.collider.gameObject == target);

            }
            //the player was not seen by the target
            return false;

            // Debug.Log(this.name + distanceToWall);

        }
   }
    
