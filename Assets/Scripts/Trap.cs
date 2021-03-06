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
    public float spikeTimer;
    public float spikeTimerDown;
    public GameObject partSystem;
        

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(detectionPlayer.transform.position, lineOfSite, 0, playerLayer);
    }

    void Update()
    {
        lavaTimer += Time.deltaTime;

        if (isASpikeTrap) //SPIKE
        {
            if (canSeePlayer) //WHEN SPIKE CAN SEE PLAYER
            {
                spikeTimer += Time.deltaTime;
                partSystem.SetActive(true);

                if (spikeTimer >= spikeTimerDown)
                {
                    rb.velocity = -transform.up * TrapSpeed;
                    partSystem.SetActive(false);
                }                
            }
            else
            {
                partSystem.SetActive(false);
            }
        }

        if (isAFireTrap) //WHEN IS A LAVA CREATOR
        {
            if(lavaTimer >= timerBetweenLavaDrop)
            {
                Instantiate(lava, lavaCreator.position, transform.rotation);
                lavaTimer = 0;
            }
            
        }
    }

    //KNOCKBACK PLAYER
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

    //DESTROY SPIKE TRAP
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.name == "Spike_Trap")
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPlayer.transform.position, lineOfSite);
    }
}
