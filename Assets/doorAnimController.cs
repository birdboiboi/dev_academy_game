using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimController : MonoBehaviour
{

    public Animator _animator = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("end" + collider.gameObject.name);
        _animator.SetBool("isopen", true);
        // Application.Quit();



    }
}
