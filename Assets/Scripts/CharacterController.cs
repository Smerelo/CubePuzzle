using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]

public class CharacterController : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private const float skinWidth = 0.15f;
    private RaycastOrigins raycastOrigins;
    private float horizontalRaySpacing,
                  verticalRaySpacing;

    [Header("Collisions")]
    [SerializeField] private LayerMask collisionMask;

    [Header("Raycast")]
    [SerializeField] int horizontalRayCount = 4;
    [SerializeField] int verticalRayCount = 4;
    [HideInInspector] public CollisionFlags collisions;
    

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySapcing();
    }

    internal void Move(Vector3 deltaPos)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (deltaPos.x != 0)
            HorizontalCollisions(ref deltaPos);
        if (deltaPos.y != 0)
            VerticalCollisions(ref deltaPos);

        transform.Translate(deltaPos);
    }

    private void VerticalCollisions(ref Vector3 deltaPos)
    {
        float directionY = Mathf.Sign(deltaPos.y);
        float rayLenght = Mathf.Abs(deltaPos.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = directionY == -1 ? raycastOrigins.botomLeft : raycastOrigins.topleft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + deltaPos.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY,rayLenght, collisionMask);
            if (hit && !hit.collider.isTrigger)
            {
                deltaPos.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;
                collisions.down = directionY == -1;
                collisions.up = directionY == 1;
            }
        }
    }  
    private void HorizontalCollisions(ref Vector3 deltaPos)
    {
        float directionX = Mathf.Sign(deltaPos.x);
        float rayLenght = Mathf.Abs(deltaPos.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = directionX == -1 ? raycastOrigins.botomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);
            if (hit &&!hit.collider.isTrigger)
            {
                deltaPos.x = (hit.distance - skinWidth) * directionX;
                rayLenght = hit.distance;
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;


            }
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        raycastOrigins.botomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topleft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySapcing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topleft, topRight;
        public Vector2 botomLeft, bottomRight;
    }


    public struct CollisionFlags
    {
        public bool up, down,
                    left, right;

        public void Reset()
        {
            up = down = false;
            left = right = false;
        }
    }
   
}
