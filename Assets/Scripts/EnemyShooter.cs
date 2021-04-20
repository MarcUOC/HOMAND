using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Vector2 lineOfSite;
    public LayerMask playerLayer;
    private bool canSeePlayer;
    public GameObject rockBullet;
    public Transform firingPoint;

    public Transform player;
    Rock rockDirection;
    public GameObject rock;



    public float timer;
    public float timeBetweenRock;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeePlayer)
        {
            timer += Time.deltaTime;
            float distanceFromPlayer = player.position.x - transform.position.x;                      

            if (distanceFromPlayer < 0)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                if (rock != null && (timer >= timeBetweenRock + 0.5f)) { rockDirection.leftDirection = false; }             
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                if (rock != null && (timer >= timeBetweenRock + 0.5f)) { rockDirection.leftDirection = true; }             
            }

            if (timer >= timeBetweenRock)
            {
                Instantiate(rockBullet, firingPoint.position, transform.rotation);
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, lineOfSite);
    }
}
