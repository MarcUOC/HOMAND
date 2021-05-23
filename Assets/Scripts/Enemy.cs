using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("ALL ENEMIES")]
    public float hp;
    public float speed;
    public bool frozenEnemy;
    public float frozenTime;
    public float frozenMaxTime;
    public Vector2 lineOfSite;
    public Rigidbody2D rb;
    private bool canSeePlayer;
    public Transform firingPoint;
    public GameObject detectionPlayer;
    public LayerMask playerLayer;

    [Header("SHOOTER ENEMY")]
    public bool isAShooter;
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public GameObject rock;
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
    public Transform groundDetection;
    private bool movingRight = true;

    [Header("INVOKER ENEMY")]
    public bool isAnInvoker;    
    public GameObject bombPrefab;
    public float invokeTimer;
    public float timeForInvoke;

    [Header("BOMB ENEMY")]
    public bool isABomb;

    [Header("ANIMATIONS")]
    public Animator anim;
    private SpriteRenderer spriteHurt;
    private float resetHurt;
    public BoxCollider2D boxCol1;
    public BoxCollider2D boxCol2;
    public GameObject freezingSpikes;

    [Header("BOSS")]
    public bool isABoss;
    public float timerBoss;
    public GameObject finalDoor;
    public GameObject partSystemDoor;
    public bool bossIsDeath;
    public Image healthBar;
    public float maxHealthBoss;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteHurt = GetComponent<SpriteRenderer>();
        originalSpeed = speed;
    }

    private void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(detectionPlayer.transform.position, lineOfSite, 0, playerLayer);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }


    void Update()
    {
        if (!frozenEnemy && isAnInvoker && hp > 0)
        {
            if (canSeePlayer)
            {
                speed = 0;
                invokeTimer += Time.deltaTime;

                if (invokeTimer >= timeForInvoke)
                {                    
                    if (!isABoss)
                    {
                        anim.SetBool("Invoker Attack", true);
                        Instantiate(bombPrefab, firingPoint.position, transform.rotation);
                        timeForInvoke = Random.Range(0.5f, 1.5f);
                        invokeTimer = 0;
                    }
                    if (isABoss)
                    {
                        anim.SetBool("Boss Attack",true);
                        anim.SetBool("Boss Walk", false);
                        Instantiate(bombPrefab, firingPoint.position, transform.rotation);
                        invokeTimer = 0;
                    }
                }
            }
        }        

        if (!frozenEnemy && isABomb && hp > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            //transform.Rotate(new Vector3(0, 0, 100f) * Time.deltaTime);

            /*if (transform.rotation.y == 0)
            {
                transform.Translate(Vector3.right * Time.deltaTime);
            }
            else
            {
                
            }  */      
        }

        if (!frozenEnemy && isAShooter && hp > 0)
        {
            if (canSeePlayer)
            {
                timeForShoot += Time.deltaTime;
                timeForJump += Time.deltaTime;

                if (timeForShoot >= timeBetweenRock)
                {
                    if (!isABoss)
                    {
                        anim.SetBool("Attack", true);
                        Instantiate(rock, firingPoint.position, transform.rotation);
                        timeForShoot = 0;
                        timeBetweenRock = Random.Range(0.4f, 1f);
                    }
                    if (isABoss)
                    {
                        anim.SetBool("Boss Attack", true);
                        anim.SetBool("Boss Walk", false);
                        Instantiate(rock, firingPoint.position, transform.rotation);
                        timeBetweenRock = Random.Range(0.25f, 0.5f);
                        timeForShoot = 0;
                    }
                }

                if (isGrounded && (timeForJump >= timeBetweenJump))
                {
                    isGrounded = false;
                    rb.velocity = Vector2.up * jumpForce;
                    timeForJump = 0;
                    timeBetweenJump = Random.Range(0.25f, 2.5f);
                }
            }
            else
            {
                if (isABoss)
                {
                    anim.SetBool("Boss Idle", true);
                }

                //timeForShoot = 0;
                flipTimer += Time.deltaTime;
                if (flipTimer <= timeForFlip) { transform.eulerAngles = new Vector3(0, -180, 0); }
                if (flipTimer > timeForFlip) { transform.eulerAngles = new Vector3(0, 0, 0); }
                if (flipTimer >= timeForResetFlip) { flipTimer = 0; }
            }
        }

        if (!frozenEnemy && isAChaser && hp > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f);
            alert.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z);
            Debug.DrawRay(groundDetection.position, Vector2.down, Color.white);

            if (groundInfo.collider == false)
            {
                Flip();
            }

            if (canSeePlayer)
            {
                chasetimer += Time.deltaTime;
                speed = 0;
                alert.gameObject.SetActive(true);
                if (!isABoss) { anim.SetBool("Start Chasing", true); }
                if (isABoss) { anim.SetBool("Boss Idle", true); }

                if (chasetimer >= timeToStartChasing)
                {
                    speed = speedWhenPlayerSpotted;
                    alert.gameObject.SetActive(false);
                    if (!isABoss) { anim.SetBool("Run", true); }
                    if (isABoss) { anim.SetBool("Boss Run", true); }
                }
            }
            else
            {
                chasetimer = 0;
                speed = originalSpeed;
                alert.gameObject.SetActive(false);
                if (!isABoss) { anim.SetBool("Start Chasing", false); anim.SetBool("Run", false); }
                if (isABoss) { anim.SetBool("Boss Idle", false); anim.SetBool("Boss Run", false); }
            }
        }
        else if (frozenEnemy && isAChaser && hp > 0)
        {
            alert.gameObject.SetActive(false);
        }
        
        if (!frozenEnemy && isABoss && hp > 0)
        {
            timerBoss += Time.deltaTime;
            if (timerBoss < 10)
            {
                isAChaser = true;
            }

            if (timerBoss > 10 && timerBoss < 20)
            {
                isAChaser = false;
                isAShooter = true;
            }

            if (timerBoss > 20 && timerBoss < 25)
            {
                isAShooter = false;
                isAChaser = true;
            }

            if (timerBoss > 25 && timerBoss < 35)
            {
                isAChaser = false;
                isAnInvoker = true;
            }

            if (timerBoss > 35)
            {
                isAnInvoker = false;
                isAChaser = true;
                timerBoss = 0;
            }

            if (!canSeePlayer && isAnInvoker)
            {
                flipTimer += Time.deltaTime;
                if (flipTimer <= timeForFlip) { transform.eulerAngles = new Vector3(0, -180, 0); }
                if (flipTimer > timeForFlip) { transform.eulerAngles = new Vector3(0, 0, 0); }
                if (flipTimer >= timeForResetFlip) { flipTimer = 0; }
            }
        }

        if (frozenEnemy && hp > 0 && !isABomb)
        {
            anim.enabled = false;
            freezingSpikes.SetActive(true);

            frozenTime += Time.deltaTime;
            if(frozenTime >= frozenMaxTime)
            {
                frozenEnemy = false;
                frozenTime = 0;
            }
        }
        else
        {
            anim.enabled = true;
            freezingSpikes.SetActive(false);
        }        

        if ((isAShooter || isABoss) && isGrounded && hp <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            if (bossIsDeath)
            {
                Color color = GetComponent<SpriteRenderer>().material.color;
                color.a -= Time.deltaTime * 0.40f;
                GetComponent<SpriteRenderer>().material.color = color;

                partSystemDoor.transform.position = transform.position;
                finalDoor.transform.position = transform.position;
                partSystemDoor.SetActive(true);
            }
        }

        if (isABoss)
        {
            healthBar.fillAmount = hp / maxHealthBoss;
        }

        if (spriteHurt.color == new Color(255, 0, 0, 255)) //color red
        {
            resetHurt += Time.deltaTime;
            if (resetHurt >= 0.15f)
            {
                spriteHurt.color = new Color(255, 255, 255, 255); //color white
                resetHurt = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //collision bullet vs enemy
        if (other.gameObject.tag.Equals("FireBall"))
        {
            hp = hp - 1;

            if (!isABomb)
            {
                spriteHurt.color = new Color(255, 0, 0, 255);
            }

            if (isAChaser && !isABoss)
            {
                if (!canSeePlayer && !frozenEnemy)
                {
                    Flip();
                }

                if(hp <= 0)
                {
                    anim.SetBool("Death", true);
                    boxCol1.enabled = false;
                    boxCol2.enabled = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Destroy(alert.gameObject);
                }                
            }

            if (isAChaser && isABoss)
            {
                if (!canSeePlayer && !frozenEnemy)
                {
                    Flip();
                }
            }

            if (isAShooter && hp <= 0 && !isABoss)
            {
                anim.SetBool("Death", true);
                boxCol1.enabled = false;
                boxCol2.enabled = false;
            }

            if (isAnInvoker && hp <= 0 && !isABoss)
            {
                anim.SetBool("Invoker Death", true);
                boxCol1.enabled = false;
                boxCol2.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (isABomb && hp <= 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCol1.enabled = false;
                anim.SetBool("Explosion", true);
            }

            if (isABoss && hp <= 0)
            {
                anim.SetBool("Boss Death", true);
                boxCol1.enabled = false;
                boxCol2.enabled = false;
                bossIsDeath = true;      
            }
        }        

        //collision orb vs enemy
        if (other.gameObject.tag.Equals("Orb"))
        {
            frozenEnemy = true;

            if (isABomb)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCol1.enabled = false;
                anim.SetBool("Explosion", true);
            }
        }

        //collision enemy vs enemy or wall
        if (other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Wall")) 
        {
            if (!frozenEnemy && isAChaser)
            {
                Flip();
            }

            if (!frozenEnemy && isABomb)
            {
                hp = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCol1.enabled = false;
                anim.SetBool("Explosion", true);
            }
        }

        if (other.gameObject.tag.Equals("Player") && isABomb)
        {
            hp = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            boxCol1.enabled = false;
            anim.SetBool("Explosion", true);
        }
    }

    void EnemyDie()
    {
        Destroy(gameObject);
    }

    void BossDie()
    {
        partSystemDoor.SetActive(false);
        finalDoor.SetActive(true);
        Destroy(gameObject);
    }

    void ResetAttack()
    {
        if (isAShooter && !isABoss) { anim.SetBool("Attack", false); }
        if (isAnInvoker && !isABoss) { anim.SetBool("Invoker Attack", false); }
        if (isABoss) { anim.SetBool("Boss Attack", false);  }        
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
            else
            {
                player.knockFromRight = false;
            }
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
