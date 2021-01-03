using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAnimation : MonoBehaviour
{
    private bool doortoggle = false;

        public Animator _animator = null;

    void OnTriggerEnter(Collider collider)
    {
        _animator.SetBool("isopen", true);
        Debug.Log("OpenDoor");
    }

    void OnTriggerExit(Collider collider)
    {
        _animator.SetBool("isopen", false);
        Debug.Log("CloseDoor");
    }
    }