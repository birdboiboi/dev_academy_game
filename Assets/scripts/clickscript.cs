using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clickscript : MonoBehaviour
{

    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
     //get the renderer component of this object
    rend = GetComponent<Renderer>();
    //update mouse from its lockedstate
    Cursor.lockState = CursorLockMode.Confined;
    }

    

    void OnMouseEnter()
    {
        //change the text's color when the mouse hovers over the colider
        rend.material.color = Color.red;
    }
    void OnMouseDown()
    {
        //clicking the colider will change the color blue
        rend.material.color = Color.blue;
        

    }
    void OnMouseUp()
    {
        //load the main menue
        SceneManager.LoadScene("main_menu");
    }
    void OnMouseExit()
    {
        //if the mouse does not click and is removed from the colider the original color of the text will be restored
        rend.material.color = Color.white;
    }
}