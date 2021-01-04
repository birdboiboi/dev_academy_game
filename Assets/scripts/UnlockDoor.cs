using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public triggerSelect trigger;
    public Collider animDoor;
    // Start is called before the first frame update
    void Start()
    {
        animDoor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.isTriggered)
        {
            animDoor.enabled = true;
        }
    }
}
