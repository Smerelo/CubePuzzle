using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] GameObject[] cameras;
    
    private int currentCamera;


    // Start is called before the first frame update
    void Start()
    {
        currentCamera = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SwitchCamera(int newCamera)
    {
        if (newCamera == currentCamera)
        {
            return;
        }
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == newCamera)
            {
                currentCamera = i;
                cameras[i].gameObject.SetActive(true);
                continue;
            }
            cameras[i].gameObject.SetActive(false);
        }
    }
}
