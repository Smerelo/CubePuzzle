using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager cameraManager;
    private SidesManager sides;
    private Face face;
    void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        sides = GameObject.Find("Map").GetComponent<SidesManager>();
        face = transform.parent.parent.gameObject.GetComponent<Face>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            cameraManager.SwitchCamera(face.FaceNb);
            sides.SwitchSides(face);
        }
    }
}
