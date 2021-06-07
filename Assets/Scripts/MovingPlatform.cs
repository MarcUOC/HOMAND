using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed;
    public Transform startPoint;
    Vector3 nextPoint;

    void Start()
    {
        nextPoint = startPoint.position;
    }

    void Update()
    {
        //TRANSFORM POSITION OF THE PLATFORM POINT A
        if (transform.position == pointA.position)
        {
            nextPoint = pointB.position;
        }
        //TRANSFORM POSITION OF THE PLATFORM POINT B
        if (transform.position == pointB.position)
        {
            nextPoint = pointA.position;
        }

        //MOVE PLATFORM
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
