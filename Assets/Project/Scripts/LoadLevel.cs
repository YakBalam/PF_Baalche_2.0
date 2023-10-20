using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public int sceneX;
    public Canvas canvas;
    private bool flag = false;
    
    void CargarEscena(int escena)
    {
        SceneManager.LoadScene(escena);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.enabled = true;
            flag = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.enabled = false;
            flag = false;
        }
    }

    void Update()
    {
        if(flag==true)
        {
            if(Input.GetKeyUp(KeyCode.C))
            {
                CargarEscena(sceneX);
            }
        }
    }
}
