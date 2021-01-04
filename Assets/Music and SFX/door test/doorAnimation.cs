using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimation : MonoBehaviour
{
    private bool doortoggle = false;

        public Animator _animator = null;

    void OnTriggerEnter(Collider collider)
    {
        doortoggle = true;
        _animator.SetBool("isopen", doortoggle);
        Debug.Log("OpenDoor");
    }

    void OnTriggerExit(Collider collider)
    {
        doortoggle = false;
        _animator.SetBool("isopen", doortoggle);
        Debug.Log("CloseDoor");
    }
    }