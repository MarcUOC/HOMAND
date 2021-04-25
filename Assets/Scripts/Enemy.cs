using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ALL ENEMIES")]
    public int hp;
    public bool frozenEnemy;
    public float frozenTime;
    public float frozenMaxTime;

    [Header("PATROL ENEMY")]
    public bool isAPatrol;
    public float speed;
    private bool movingRight = true;
    public Rigidbody2D rb;
    public Transform groundDetection;

    [Header("SHOOTER ENEMY")]
    public bool isAShooter;
    public Vector2 lineOfSite;
    public LayerMask playerLayer;
    private bool canSeePlayer;
    public GameObject rock;
    public Transform firingPoint;
    public Transform player;
    public GameObject detectionPlayer;
    public float jumpForce;
    public float timeForShoot;
    public float timeBetweenRock;
    public float timeForJump;
    public float timeBetweenJump;    
    public float flipTimer;
    public float timeForFlip;
    public float timeForResetFlip;

    [Header("CHASER ENEMY")]
    public bool isAChaser;
    public float chasetimer;
    public float timeToStartChasing;
    public float speedWhenPlayerSpotted;
    private float originalSpeed;
    public GameObject alert;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;        
    }

    private void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(detectionPlayer.transform.position, lineOfSite, 0, playerLayer);
    }


    void Update()
    {
        if (!frozenEnemy && isAPatrol)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f);
            Debug.DrawRay(groundDetection.position, Vector2.down,Color.white);

            if (groundInfo.collider == false)
            {
                Flip();
            }
        }

        if (!frozenEnemy && isAShooter)
        {
            if (canSeePlayer)
            {
                timeForShoot += Time.deltaTime;
                timeForJump += Time.deltaTime;

                if (timeForShoot >= timeBetweenRock)
                {
                    Instantiate(rock, firingPoint.position, transform.rotation);
                    timeForShoot = 0;
                    timeBetweenRock = Random.Range(0.25f, 1);
                }

                if (timeForJump >= timeBetweenJump)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    timeForJump = 0;
                    timeBetweenJump = Random.Range(1.5f, 5);
                }
            }
            else
            {
                timeForShoot = 0;
                flipTimer += Time.deltaTime;
                if (flipTimer <= timeForFlip) { transform.eulerAngles = new Vector3(0, -180, 0); }
                if (flipTimer > timeForFlip) { transform.eulerAngles = new Vector3(0, 0, 0); }
                if (flipTimer >= timeForResetFlip) { flipTimer = 0; }
            }
        }

        if (!frozenEnemy && isAChaser)
        {
            isAPatrol = true;
            alert.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+0.35f, this.transform.position.z);
           
            if (canSeePlayer)
            {
                chasetimer += Time.deltaTime;
                speed = 0;
                alert.gameObject.SetActive(true);

                if (chasetimer >= timeToStartChasing)
                {
                    speed = speedWhenPlayerSpotted;
                    alert.gameObject.SetActive(false);
                }           
            }
            else
            {
                chasetimer = 0;
                speed = originalSpeed;
                alert.gameObject.SetActive(false);
            }
        }

        if (frozenEnemy)
        {
            frozenTime += Time.deltaTime;
            if(frozenTime >= frozenMaxTime)
            {
                frozenEnemy = false;
                frozenTime = 0;
            }
        }
    }
    

    void OnCollisionEnter2D(Collision2D other)
    {
        //collision bullet vs enemy
        if (other.gameObject.tag.Equals("FireBall"))
        {
            hp--; //Enemy -1 hp
            if (hp <= 0)
            {
                Destroy(gameObject); //Enemy die
            }

            if (isAChaser && !canSeePlayer && !frozenEnemy)
            {
                Flip();
            }
        }

        //collision orb vs enemy
        if (other.gameObject.tag.Equals("Orb"))
        {
            frozenEnemy = true;            
        }


        //collision enemy vs enemy or wall
        if (other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Wall")) 
        {
            if (!frozenEnemy)
            {
                Flip();
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

    public void Flip()
    {
        if (movingRight == true)
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPlayer.transform.position, lineOfSite);
    }
}
