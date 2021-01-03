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

        Vector3 screenPoint = cam.WorldToViewportPoint(target.transform.position);
        Debug.Log("player is seen" + playerSeen());
        onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && playerSeen();
        Debug.Log("player is screen" + onScreen);
        if (onScreen)
        {
            if (onScreen != oldOnScreen)
            {
                targetScript.numReflection++;
                oldOnScreen = onScreen;
            }

        }
        else
        {
            if (oldOnScreen != onScreen)
            {
                targetScript.numReflection -= 1;
                oldOnScreen = onScreen;
                targetScript.ResetThismonster();

            }

        }
    }

        bool playerSeen()
        {

            Vector3 dirVect = -Vector3.Normalize(transform.position - target.transform.position);


            RaycastHit hit;
            Ray distWall = new Ray(transform.position, dirVect);
            if (Physics.Raycast(distWall, out hit))
            {
                
                Debug.DrawRay(cam.WorldToViewportPoint(target.transform.position), dirVect * hit.distance, Color.yellow);
                Debug.Log(hit.collider.gameObject.name + distanceToWall);
                Debug.DrawRay(transform.position, dirVect * distanceToWall, Color.yellow);
                return (hit.collider.gameObject == target);

            }
            //Debug.Log("none");
            return false;

            // Debug.Log(this.name + distanceToWall);

        }
   }
    
