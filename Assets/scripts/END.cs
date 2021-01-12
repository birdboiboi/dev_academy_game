using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class END : MonoBehaviour
{
    public GameObject player;
    public Animator _animator ;
    // Start is called before the first frame update
  
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("end" + collider.gameObject.name);


        //animate the door
        _animator.SetBool("isopen", true);
        // Application.Quit();

        StartCoroutine(transition());

    }

    IEnumerator transition()
    {

       //to see the door animation and end the scene after minor delay
        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene("Ending");



    }
}
