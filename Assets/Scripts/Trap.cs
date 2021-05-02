using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public bool isASpikeTrap;
    public bool isAFireTrap;
    public float TrapSpeed;
    public Rigidbody2D rb;
    public Vector2 lineOfSite;
    private bool canSeePlayer;
    public GameObject detectionPlayer;
    public LayerMask playerLayer;
    public Transform lavaCreator;
    public GameObject lava;

    public float lavaTimer;
    public float timerBetweenLavaDrop;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(detectionPlayer.transform.position, lineOfSite, 0, playerLayer);
    }

    // Update is called once per frame
    void Update()
    {
        lavaTimer += Time.deltaTime;

        if (isASpikeTrap)
        {
            if (canSeePlayer)
            {
                rb.velocity = -transform.up * TrapSpeed;
                Destroy(gameObject, 3);
            }
        }

        if (isAFireTrap)
        {
            if(lavaTimer >= timerBetweenLavaDrop)
            {
                Instantiate(lava, lavaCreator.position, transform.rotation);
                lavaTimer = 0;
            }
            
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            var player = other.GetComponent<Player>();
            player.knockbackCount = player.knockbackLength;

            if (other.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else { player.knockFromRight = false; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPlayer.transform.position, lineOfSite);
    }

    /*void CreateLava()
    {
        Instantiate(lava, lavaCreator.position, transform.rotation);
    }*/
}
