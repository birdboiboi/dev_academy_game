using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandle : MonoBehaviour
{


    //Setup
    private Camera cam;


    public Shader normalShader;
    public Transform body;
    public GameObject item = null;
    public GameObject tempItem;
    public Shader glow;

    //properties
    public float rotateSpeed = 1f;
    public float sensitiveity = 100;
    public float distHold;
    private float xRot = 0;
    private bool isRotate = false;


    //


    // Start is called before the first frame update
    void Start()
    {
        //lock the mouse to the screen to remove distracting nature of mouse
        Cursor.lockState = CursorLockMode.Locked;

        //setup camera of player
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        //************************MOVEMENT**********************************

        //get yaw as mouse verticle location and pitch as mouse horizontal location
        float yaw = Input.GetAxis("Mouse Y") * sensitiveity * Time.deltaTime;
        float pitch = Input.GetAxis("Mouse X") * sensitiveity * Time.deltaTime;
        xRot -= yaw;

        //Limits to prevent 360 viewing,
        xRot = Mathf.Clamp(xRot, -90, 90);

        //rotate this player in accordance to the verticle mouse movement
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        //rotate the players's non camera movement arount the y axis(horizontal movement)
        body.Rotate(pitch * Vector3.up);

        //************************GRAB OBJECTS**********************************

        //creates "00000001" where 1 is the 8th layer in the unity edittor(grabbable)
        int layerMask = 1 << 8;


        RaycastHit hit;

        //check if player is not holding an object
        if (item == null)
        {
            //make sure that player is not telling the item to rotate
            isRotate = false;

            //converting the GAMESPACE to CAMERASPACE where the middle of the screen is slightly warped due to projection math
            //camera center is taking the center of the computer screen and getting its corresponding coordinates
            Vector3 CameraCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));
            if (Physics.Raycast(CameraCenter, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                //make item in view glow, highlight the item by switching the shader
                Debug.DrawRay(CameraCenter, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                tempItem = hit.collider.gameObject;
                tempItem.GetComponent<Renderer>().material.shader = glow;

                //check if the player left clicks
                if (Input.GetButtonUp("Fire1"))
                {
                    //handle holding item

                    //with(item)
                    //{
                    item = tempItem;
                    item.GetComponent<Renderer>().material.shader = glow;
                    item.transform.SetParent(this.gameObject.transform);
                    //item.GetComponent<Rigidbody>().isKinematic = false;
                    item.GetComponent<Rigidbody>().useGravity = false;
                    item.GetComponent<Collider>().enabled = false;
                    item.transform.rotation = transform.rotation;
                    item.transform.position = transform.position + cam.transform.TransformDirection(Vector3.forward) * distHold;
                    //item.GetComponent<Renderer>().material.shader = glow;
                    //}


                }

            }
            else
            {
                Debug.DrawRay(CameraCenter, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);
                //if the player looks away reapply the original shader
                if (tempItem != null)
                {
                    //Debug.Log("switch shades back");
                    tempItem.GetComponent<Renderer>().material.shader = normalShader;
                    tempItem = null;
                }
            }
        }
        else
        {

            //Handle dropping the item if the item is already held
            if (Input.GetButtonUp("Fire1"))
            {
                //using (item)
                //{
                item.GetComponent<Collider>().enabled = true;
                //item.GetComponent<Rigidbody>().isKinematic = true;
                item.GetComponent<Rigidbody>().useGravity = true;
                //item.GetComponent<Renderer>().material.shader = normalShader;
                item.transform.parent = null;

                //}

                item = null;
            }

            //rotate the item
            if (Input.GetButtonUp("Fire2"))
            {
                isRotate = !isRotate; // check state of wether to rotate or not(toggle in this line
            }
            if (isRotate && item != null)
            {
                Debug.Log("isRotate" + isRotate + "item" + item.name);
                item.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
            }

        }
    }
}