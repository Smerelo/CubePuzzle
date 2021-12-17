using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager cameraManager;
    private SidesManager sides;
    private PlayerActions player;
    private Face face;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
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
        if (collision.name == player.name)
        {
            if (sides.currentFace == face.FaceNb && !player.IsChangingZone)
            {
                return;
            }
            player.IsChangingZone = true;
            collision.transform.parent = face.transform;
            cameraManager.SwitchCamera(face.FaceNb);
            sides.SwitchSides(face);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == player.name)
        {
            if (player.IsChangingZone)
            {
                player.IsChangingZone = false;
            }
        }
    }
}
