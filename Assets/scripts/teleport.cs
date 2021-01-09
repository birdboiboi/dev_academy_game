using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public GameObject next;
    public GameObject prev;
    public GameObject player;
    public Vector3 offsetSpawn = new Vector3(0, 0, 0);
    public Vector3 offsetSpawnNext;
    public CharacterController charController;
    private CharMove playerScript;
    private MonsterTrickle monstTrick;
    public AudioSource layerThemeForMirror;

    public AudioClip warp;
    


    void Start()
    {
        charController = player.GetComponent<CharacterController>();
        playerScript = player.GetComponent<CharMove>();
        offsetSpawnNext = next.GetComponent<teleport>().offsetSpawn;

        layerThemeForMirror = GetComponent<AudioSource>();
        //monstTrick = player.transform.GetChild(1).GetComponent<MonsterTrickle>();
        //Debug.Log("howdy");
        //charController.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        
    Debug.Log(this.name);

        layerThemeForMirror.PlayOneShot(warp);
        if (col.gameObject == player)
        {
            
            Debug.Log(col.name + "trigger enter");
            //Debug.Log(col.gameObject.transform.position);
            //Debug.Log(next.position + Vector3.Scale(next.forward, offsetSpawn));
            // Instantiate(player, next.position, player.transform.rotation);

            charController.enabled = false;
            player.transform.position = next.transform.position + offsetSpawnNext;
            player.transform.rotation = transform.rotation;
            charController.enabled = true;

            playerScript.lastMirror = playerScript.thisMirror;
            playerScript.thisMirror = next;
            playerScript.ResetThismonster();
            
            StartCoroutine(transition( playerScript.layerTheme,1,0));
            


            //Destroy(col.gameObject);
            //player.transform.Translate(0, 1000, 0);
            //Destroy(player);
            //}
        }
        else
        {
            col.gameObject.transform.position = next.transform.position + offsetSpawnNext;
        }
    }

     IEnumerator transition(AudioSource src, float dur, float trgtVol)
    {
        float currentTime = 0;
        float start = src.volume;
        Debug.Log(src.volume);
        playerScript.layerTheme = layerThemeForMirror;
        playerScript.layerTheme.Play();
        while (currentTime < dur){
            currentTime += Time.deltaTime;
            src.volume = Mathf.Lerp(start, trgtVol, currentTime / dur);
            playerScript.layerTheme.volume = Mathf.Lerp(trgtVol,start, currentTime / dur);
            Debug.Log(src.volume);
            yield return null;
        }
        src.Stop();
        
        yield break;
        

    }
}
