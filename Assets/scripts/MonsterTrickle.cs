using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrickle : MonoBehaviour
{
    private CharMove playerScript;
    private CharMove masterPlayerScript;
    private CharacterController masterCharController;


    private Vector3[] dirs;
    public Vector3 dirUnitVector;


    //Setup
    public GameObject player;
    public Camera playerCamera;
    public Animator anim;
    public Animator monsterAnim;

    public AudioSource monsterVoice;
    public AudioClip[] voices;



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

        //external voice clip of the monster, multiple can be added
        monsterVoice.clip = voices[0];

        //gets the monsters current position and saves it as a start point, the monster will reset to this
        startPos = transform.localPosition;

        
        masterPlayer = GameObject.FindGameObjectWithTag("Player");
        
        //may need to remove this variable due to redundancy of masterPlayerScript but it still works so to avoid unneccasry work this variable will stay
        playerScript = player.GetComponent<CharMove>();
        masterPlayerScript = masterPlayer.GetComponent<CharMove>();
        masterCharController = masterPlayer.GetComponent<CharacterController>();

        //for spawning on a mirror when captured to avoid the being teleported. tthe player will spawn a distance away from the teleporter instead of disabling the teleporter
        offsetSpawn = masterPlayerScript.lastMirror.GetComponent<teleport>().offsetSpawn;

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
        else if((Mathf.Abs((player.transform.position - transform.position).magnitude) < killDist))//&& !isDopple)
        {
            //code for monster capturing player
           // Debug.Log("captured " + masterPlayer.name + "teleport from"+ masterPlayerScript.thisMirror.name + "  to " + masterPlayerScript.lastMirror.name + "\n from "+ masterPlayerScript.thisMirror.transform.position + " to " + masterPlayerScript.lastMirror.transform.position + offsetSpawn +"\nat " + masterPlayer.transform.position);
            //Debug.Log("player1st@" + masterPlayer.transform.position);
            masterCharController.enabled = false;
            masterPlayer.transform.position = masterPlayerScript.lastMirror.transform.position + offsetSpawn ;
            masterCharController.enabled = true;

            //reset the position of this monster(doppleganger or player's) and the makes sure the player will reset theirs
            masterPlayerScript.ResetThismonster();
            reset();

            //update spawn position with respect to the new teleport destination from being captured
            offsetSpawn = masterPlayerScript.lastMirror.GetComponent<teleport>().offsetSpawn;
            

            //update player's known position within the node list to figure out position
            masterPlayerScript.thisMirror = masterPlayerScript.thisMirror.GetComponent<teleport>().prev;
            masterPlayerScript.lastMirror = masterPlayerScript.thisMirror.GetComponent<teleport>().prev;

            //capture animation
            monsterAnim.Play("notice");

            //MUSIC TRANSITION!!!! @BJ over here!!!! this changes the music that the player is listening to the new teleporter's song ( parallel proccess)
            StartCoroutine(transition(masterPlayerScript.layerTheme, 1, 0));


        }

        if ((Mathf.Abs((player.transform.position - transform.position).magnitude) < killDist +.5f ))
        {

            //execute scream on delay
            monsterVoice.Play();
            
           // monsterVoice.clip = voices[Random.Range(0, voices.Length - 1)];
            
        }
        if (masterPlayerScript.move.x != 0 && masterPlayerScript.move.z != 0)
        {
            //make the monster animate walk if the player is walking (this condition ^^ may not work)
            anim.Play("walk");
           
        }
        else
        {
            anim.Play("idle");
           
        }

    }
    public void reset()
    {
        //reset this monster's current location, the player can invoke this too
        transform.localPosition = startPos;
       // Debug.Log("reset dopple" + transform.parent.name);
        //transform.position = -player.transform.forward * startingDist;
    }


    //allow monster to attach to nearest surfaces
    //THE MONSTER DOES NOT FLIP ORIENTATION TO PUT ITS FEET ON THE SURFACE --MUST BE IMPLEMENTED--
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


    //my own cross fade where the volume of the previous track is lowered and the volume of this track is raised
    IEnumerator transition(AudioSource src, float dur, float trgtVol)
    {
        float currentTime = 0;
        float start = src.volume;
        Debug.Log(src.volume);
        masterPlayerScript.layerTheme = masterPlayerScript.thisMirror.GetComponent<teleport>().layerThemeForMirror;
        Debug.Log("masterPlayerScript.thisMirror.name"+masterPlayerScript.thisMirror.name);


        masterPlayerScript.layerTheme.Play();
        
        while (currentTime < dur)
        {
            currentTime += Time.deltaTime;
            //smoothly lower the volume for every delta time of the while loop
            src.volume = Mathf.Lerp(start, trgtVol, currentTime / dur);
            //smoothly raise the volume for every delta time of the while loop
            masterPlayerScript.layerTheme.volume = Mathf.Lerp(trgtVol, start, currentTime / dur);

            //i tried creating a wavy effect
            masterPlayerScript.layerTheme.pitch =1* Mathf.Sin(currentTime);
           
            yield return null;
        }
        //end the previous track
        src.Stop();
        //make sure the new track is normal
        masterPlayerScript.layerTheme.pitch = 1;
        masterPlayerScript.layerTheme.volume = start;
        yield break;


        

    }
    
}
