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

    // Start is called before the first frame update
    void Start()
    {
        nextPoint = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pointA.position)
        {
            nextPoint = pointB.position;
        }
        if (transform.position == pointB.position)
        {
            nextPoint = pointA.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
