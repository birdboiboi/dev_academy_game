using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GAMEROOM1");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public Renderer m_Material;
    void Start()
    {
        //m_Material.material.mainTexturePlay();
        
        //m_Material.material.mainTexture.loop = true;
    }
}