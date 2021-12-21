using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private List<RotationStep> rotations;
    public Vector3 axis;

    private const float ANGLE = Mathf.PI / 2;
    void Start()
    {
        axis = new Vector3(1, 2, 3);
        player = GameObject.Find("Player").transform;
        rotations = new List<RotationStep>();
    }

    void Update()
    {
        RotatePosition();
    }

    internal void AddRotation(Vector3 dir, int faceNb)
    {
        RotationStep rot;
        rot.direction = dir;
        rot.axis = axis;
        rotations.Add(rot);
        axis = Rotate(dir, axis, axis);
    }

    private void RotatePosition()
    {
        Vector3 temp = GetPlayerPos();
        Vector3 pos = new Vector3(temp.x, temp.y, -16f);
        foreach (RotationStep nextRotation in rotations)
        {
            pos = Rotate(nextRotation.direction, pos, nextRotation.axis);
        }
        transform.localPosition = pos;
    }

    private Vector3 GetPlayerPos()
    {
        if (player.localEulerAngles.z != 0)
        {   
            return RotateInZ(player.localPosition, -player.localEulerAngles.z * Mathf.Deg2Rad);
        }
        return player.localPosition;
    }

    private Vector3 Rotate(Vector3 dir, Vector3 pos, Vector3 axis)
    {
        if (dir == Vector3.left)
        {
            int index = GetAxis(2, axis);
            switch (index)
            {
                case 0:
                    return RotateInX(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                case 1:
                    return RotateInY(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                case 2:
                    return RotateInZ(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                default:
                    break;
            }
        }
        else if (dir == Vector3.right)
        {

            int index = GetAxis(2, axis);
            switch (index)
            {
                case 0:
                    return RotateInX(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                case 1:
                    return RotateInY(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                case 2:
                    return RotateInZ(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                default:
                    break;
            }
        }
        else if (dir == Vector3.up)
        {

            int index = GetAxis(1, axis);
            switch (index)
            {
                case 0:
                    return RotateInX(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                case 1:
                    return RotateInY(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                case 2:
                    return RotateInZ(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                default:
                    break;
            }
        }
        else if (dir == Vector3.down)
        {

            int index = GetAxis(1, axis);
            switch (index)
            {
                case 0:
                    return RotateInX(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                case 1:
                    return RotateInY(pos, axis[index] < 0 ? -ANGLE : ANGLE);//works
                case 2:
                    return RotateInZ(pos, axis[index] > 0 ? -ANGLE : ANGLE);//works
                default:
                    break;
            }
        }
        Debug.LogError("Direction Error");
        return  new Vector3(5,5,5);
    }

    private int GetAxis(int direction,Vector3 axis)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((int)(Math.Round(Math.Abs(axis[i]))) == direction)
            {
                return i;
            }
        }
        Debug.LogError("shit doenst work");
        return -1;
    }

    private Vector3 RotateInY(Vector3 pos, float angle)
    {
        Vector3 newPos = pos;
        newPos.x = pos.x * Mathf.Cos(angle) - pos.z * Mathf.Sin(angle);
        newPos.z = pos.z * Mathf.Cos(angle) + pos.x * Mathf.Sin(angle);
        return newPos;
    }  
    private Vector3 RotateInX(Vector3 pos, float angle)
    {
        Vector3 newPos = pos;
        newPos.y = pos.y * Mathf.Cos(angle) - pos.z * Mathf.Sin(angle);
        newPos.z = pos.z * Mathf.Cos(angle) + pos.y * Mathf.Sin(angle);
        return newPos;
    }  
    private Vector3 RotateInZ(Vector3 pos, float angle)
    {
        Vector3 newPos = pos;
        newPos.x = pos.x * Mathf.Cos(angle) - pos.y * Mathf.Sin(angle);
        newPos.y = pos.y * Mathf.Cos(angle) + pos.x * Mathf.Sin(angle);
        return newPos;
    }

    private struct RotationStep
    {
        public Vector3 axis,
                       direction;
    }
}
