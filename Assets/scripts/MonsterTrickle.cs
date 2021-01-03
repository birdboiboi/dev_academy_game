using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrickle : MonoBehaviour
{
    private CharMove playerScript;
    private CharMove masterPlayerScript;

    private Vector3[] dirs;
    public Vector3 dirUnitVector;


    //Setup
    public GameObject player;
    public Camera playerCamera;

    
    //Properties
    public float startingDist= 10;
    public float movingRate = 1;
    public float killDist = 9;
    public GameObject masterPlayer;
    public bool isDopple = false;
    public Vector3 offsetSpawn ;// new Vector3(1, 0, 0);


    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        //pre write vectors of (0,1,0), (0,-1,0),(1,0,0),(-1,0,0) all orthognal to directional vector 
        dirs = new[] { transform.up, -transform.up, transform.right, -transform.right };
        
        startPos = transform.localPosition;
        masterPlayer = GameObject.FindGameObjectWithTag("Player");
        offsetSpawn = masterPlayer.GetComponent<CharMove>().thisMirror.GetComponent<teleport>().offsetSpawn;

        playerScript = player.GetComponent<CharMove>();
        masterPlayerScript = masterPlayer.GetComponent<CharMove>();


    }

    // Update is called once per frame
    void Update()
    {
        //create directional vector, need to find direction that monster must traverse to move reach player 
        dirUnitVector = Vector3.Normalize(player.transform.position - transform.position);

        //rotate this quaternion(monster's) to the coressponding angle facing the player/target
        transform.LookAt(player.transform);

        //check four orthognal locations of solid surfaces(floor,ceiling,wall right, and wall left), get the position of the closest of the four
        Vector3 monstLoc = getNearestSurface();

        //get the differece in distance between 
        float dist = Vector3.Magnitude(monstLoc - transform.GetChild(0).transform.position);
      
        //attach monstermesh(child of monster object) to the ground. unsuccessfuly lerped(linear interpalation/smoothed) position
        transform.GetChild(0).transform.position = Vector3.Lerp(monstLoc, transform.GetChild(0).transform.position, dist * .05f * Time.deltaTime);
       
        //if the player is within view AND not within a certin distance
        if ((playerScript.isSeen && (Mathf.Abs((player.transform.position - transform.position).magnitude) >= killDist))|| isDopple)
        {
            //move the player fowards(z axis) towards the player/target
            transform.Translate(0, 0, movingRate * Time.deltaTime);

        }
        else if((Mathf.Abs((player.transform.position - transform.position).magnitude) < killDist))//&& isDopple)
        {
            //code for monster capturing player
            Debug.Log("captured " + masterPlayer.name + "teleport from"+ masterPlayerScript.thisMirror.name + "  to " + masterPlayerScript.lastMirror.name + "\n from "+ masterPlayerScript.thisMirror.transform.position + " to " + masterPlayerScript.lastMirror.transform.position + offsetSpawn);
            masterPlayer.transform.position = masterPlayerScript.lastMirror.transform.position + offsetSpawn ;
            masterPlayerScript.ResetThismonster();
            
            reset();
            offsetSpawn = masterPlayerScript.thisMirror.GetComponent<teleport>().offsetSpawn;

            masterPlayerScript.thisMirror = masterPlayerScript.thisMirror.GetComponent<teleport>().prev;
            masterPlayerScript.lastMirror = masterPlayerScript.thisMirror.GetComponent<teleport>().prev;
        }

    }
    public void reset()
    {
        transform.localPosition = startPos;
        Debug.Log("reset dopple" + transform.parent.name);
        //transform.position = -player.transform.forward * startingDist;
    }

    Vector3 getNearestSurface()
    {
        //filter,output position(where to place the monster) preparatinon
        int layerMask = 1 << 9;
        Vector3 endPos = transform.position;// output for monster, default is floating
        float getDist = -1;

        //repeat four times with diffrent directions
        for (int i = 0; i < 4; i++)
        {
            //look up raycast, imagine draing a line from a position in a direction and seing wherer the line stops
            RaycastHit hit;
            Ray distWall = new Ray(transform.position, dirs[i]);
            if (Physics.Raycast(distWall, out hit, Mathf.Infinity, layerMask))
            {
                
                 if (getDist == -1)//first direction is default
                 {
                     getDist = hit.distance;
                     endPos = hit.point;
                 }
                 else if(getDist > hit.distance) //if a closer surface is found choose the closer of the two as the new surface to place the monster
                 {
                     endPos = hit.point;
                 }
             }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);

            //endPos = hit.point;
        }
           
            
        
        return endPos; // output the closest surface to place the monster
    }
}
