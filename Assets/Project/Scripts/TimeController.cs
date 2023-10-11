using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject mainPausaPanel;
    public GameObject mainPoderPanel;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainPausaPanel.activeSelf == true)
        {
            if (Time.timeScale > 0f)
                Time.timeScale = 0f;
        }
        else if (mainPoderPanel.activeSelf == true)
        {
            if (Time.timeScale != 0.5f)
                Time.timeScale = 0.5f;
        }
        else
        {
            if (Time.timeScale < 1f)
                Time.timeScale = 1f;
        }
    }
}
