using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderAI : MonoBehaviour
{
    public GameObject target;
    public float chaseSpeed;
    private CharMove playerScript;
    public Vector3 offsetSpawn;
    private CharacterController charCont;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = target.GetComponent<CharMove>();
        charCont = target.GetComponent<CharacterController>();
        offsetSpawn = playerScript.lastMirror.GetComponent<teleport>().offsetSpawn;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(target.transform.position.x,transform.position.y, target.transform.position.z));
        if (playerSeen())
        {
            transform.Translate(transform.forward * chaseSpeed * Time.deltaTime);
        }
    }

    bool playerSeen()
    {

        Vector3 dirVect = -Vector3.Normalize(transform.position - target.transform.position);


        RaycastHit hit;
        Ray distWall = new Ray(transform.position, dirVect);
        Debug.DrawRay(target.transform.position, dirVect * 100, Color.yellow);
        if (Physics.Raycast(distWall, out hit))
        {
            
            Debug.DrawRay(target.transform.position, dirVect * hit.distance, Color.yellow);
           // Debug.Log(hit.collider.gameObject.name + distanceToWall);
            //Debug.DrawRay(transform.position, dirVect * distanceToWall, Color.yellow);
            return (hit.collider.gameObject == target);

        }
        //Debug.Log("none");
        return false;

        // Debug.Log(this.name + distanceToWall);

    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("spider COLLLOOODDEEE" + collision.gameObject.name);
        if(collision.gameObject == target)
        {
            Debug.Log("spider hit theee playyyaaa" + collision.gameObject.name);
            charCont.enabled = false;
            target.transform.position = playerScript.lastMirror.transform.position+ offsetSpawn;
            charCont.enabled = true;

        }
    }
    
}
