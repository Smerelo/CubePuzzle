using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidesManager : MonoBehaviour
{
    private Vector3 diff;
    [HideInInspector]
    public int currentFace;
    [SerializeField] Face[] sides;
    [SerializeField] Transform[] cameras;
    private const float OFFSET = 32f;
    void Start()
    {
        diff = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SwitchSides(Face side)
    {
        if (currentFace == side.FaceNb)
        {
            return;
        }
        currentFace = side.FaceNb;
        Vector3 nextSidePos = side.FacePos;
        for (int i = 0; i < sides.Length; i++)
        {
            if (nextSidePos.x != 0)
            {
                sides[i].FacePos -= nextSidePos;
                float tempx = sides[i].FacePos.x;
                if (tempx > 2 || tempx < -1)
                    sides[i].FacePos.x = tempx == 3 ? -1 : 2;
                sides[i].transform.localPosition = sides[i].FacePos * OFFSET;
                continue;
            }
               
        }
    }



    /*internal void SwitchSides(Face side)
    {
        if (currentFace == side.FaceNb)
        {
            return;
        }
        currentFace = side.FaceNb;
        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i].FaceNb == side.FaceNb)
            {
                continue;
            }
            diff = sides[i].FacePos - side.FacePos;
            if (diff.x == 0 || diff.y == 0)
            {
                double offset = diff.x == 0 ? diff.y : diff.x;
                if (offset >= 3 || offset <= -2)
                {
                    
                    sides[i].transform.localPosition += side.FacePos * OFFSET * 4;
                    sides[i].FacePos += side.FacePos * 4 - side.FacePos;
                    continue;
                }

            }
            else 
            {
                if (diff.magnitude > 2.2f)
                {
                    sides[i].transform.localPosition -= (diff - side.FacePos) * OFFSET;
                    sides[i].transform.Rotate(0, 0, 180);
                    cameras[i].RotateAround(cameras[i].position, cameras[i].forward,180);
                    sides[i].FacePos -= diff;
                    continue;
                }
                else
                {
                    float dir = Vector3.Cross(sides[i].FacePos, side.FacePos).z;
                    sides[i].transform.localPosition += side.FacePos * OFFSET;
                    sides[i].transform.Rotate(0, 0, 90 * dir);
                    cameras[i].RotateAround(cameras[i].position,cameras[i].forward, 90* -dir);
                    continue;
                }
            }
            sides[i].FacePos -= side.FacePos;
        }
        side.FacePos = Vector3.zero;
    }*/
}
