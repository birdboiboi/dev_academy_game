using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorFinal : MonoBehaviour
{
    public triggerSelect trigger1;
    public triggerSelect trigger2;
    public Collider animDoor;
    // Start is called before the first frame update
    void Start()
    {
        animDoor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger1.isTriggered && trigger2.isTriggered)
        {

            animDoor.enabled = true;
        }
    }
}
