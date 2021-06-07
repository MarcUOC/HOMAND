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
    public AudioSource frozenSound;

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
    public AudioSource shooterDie;

    [Header("CHASER ENEMY")]
    public bool isAChaser;
    public float chasetimer;
    public float timeToStartChasing;
    public float speedWhenPlayerSpotted;
    private float originalSpeed;
    public GameObject alert;
    public Transform groundDetection;
    private bool movingRight = true;
    public AudioSource chaserDie;

    [Header("INVOKER ENEMY")]
    public bool isAnInvoker;    
    public GameObject bombPrefab;
    public float invokeTimer;
    public float timeForInvoke;
    public AudioSource invokerDie;

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
    public AudioSource bossDie;


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
        if (!frozenEnemy && isAnInvoker && hp > 0) //INVOKER - BOSS MECHANICS
        {
            if (canSeePlayer) //WHEN ENEMY CAN SEE THE PLAYER
            {
                speed = 0;
                invokeTimer += Time.deltaTime;

                if (invokeTimer >= timeForInvoke)
                {                    
                    if (!isABoss) //INVOKER ATTACK
                    {
                        anim.SetBool("Invoker Attack", true);
                        Instantiate(bombPrefab, firingPoint.position, transform.rotation);
                        timeForInvoke = Random.Range(0.5f, 1.5f);
                        invokeTimer = 0;
                    }
                    if (isABoss) //BOSS ATTACK
                    {
                        anim.SetBool("Boss Attack",true);
                        anim.SetBool("Boss Walk", false);
                        anim.SetBool("Boss Run", false);
                        Instantiate(bombPrefab, firingPoint.position, transform.rotation);
                        invokeTimer = 0;
                    }
                }
            }
            else //WHEN ENEMY CAN'T SEE THE PLAYER
            {
                if (isABoss) //BOSS RESET ANIMATIONS
                { 
                    anim.SetBool("Boss Walk", false);
                    anim.SetBool("Boss Idle", true);
                    anim.SetBool("Boss Attack", false);
                }
            }
        }        

        if (!frozenEnemy && isABomb && hp > 0) //BOMB MECHANICS
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);   
        }

        if (!frozenEnemy && isAShooter && hp > 0) //SHOOTER - BOSS MECHANICS
        {
            if (canSeePlayer) //WHEN ENEMY CAN SEE THE PLAYER
            {
                timeForShoot += Time.deltaTime;
                timeForJump += Time.deltaTime;

                if (timeForShoot >= timeBetweenRock)
                {
                    if (!isABoss) //SHOOTER ATTACK
                    {
                        anim.SetBool("Attack", true);
                        Instantiate(rock, firingPoint.position, transform.rotation);
                        timeForShoot = 0;
                        timeBetweenRock = Random.Range(0.4f, 1f);
                    }
                    if (isABoss) //BOSS ATTACK
                    {
                        anim.SetBool("Boss Attack", true);
                        anim.SetBool("Boss Walk", false);
                        anim.SetBool("Boss Run", false);
                        Instantiate(rock, firingPoint.position, transform.rotation);
                        timeBetweenRock = Random.Range(0.25f, 0.5f);
                        timeForShoot = 0;
                    }
                }

                if (isGrounded && (timeForJump >= timeBetweenJump)) //WHEN GROUNDED -> RANDOM JUMP
                {
                    isGrounded = false;
                    rb.velocity = Vector2.up * jumpForce;
                    timeForJump = 0;
                    timeBetweenJump = Random.Range(0.25f, 2.5f);
                }
            }
            else //WHEN ENEMY CAN'T SEE THE PLAYER
            {
                if (isABoss) //RESET BOSS ANIMATIONS
                {
                    anim.SetBool("Boss Idle", true);
                    anim.SetBool("Boss Walk", false);
                    anim.SetBool("Boss Attack", false);
                }

                //FLIP
                flipTimer += Time.deltaTime;
                if (flipTimer <= timeForFlip)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                }
                if (flipTimer > timeForFlip)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                if (flipTimer >= timeForResetFlip)
                {
                    flipTimer = 0;
                }
            }
        }

        if (!frozenEnemy && isAChaser && hp > 0) //CHASER - BOSS MECHANICS
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f);
            alert.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z);
            //Debug.DrawRay(groundDetection.position, Vector2.down, Color.white);

            if (groundInfo.collider == false)
            {
                Flip();
            }

            if (canSeePlayer) //WHEN ENEMY CAN SEE THE PLAYER
            {
                chasetimer += Time.deltaTime;
                speed = 0;
                alert.gameObject.SetActive(true);

                if (!isABoss) //CHASER ANIMATION
                {
                    anim.SetBool("Start Chasing", true);
                }

                if (isABoss) //BOSS ANIMATIONS
                {
                    anim.SetBool("Boss Walk", false);
                    anim.SetBool("Boss Idle", true);
                }

                if (chasetimer >= timeToStartChasing)
                {
                    speed = speedWhenPlayerSpotted;
                    alert.gameObject.SetActive(false);

                    if (!isABoss) //CHASER ANIMATION
                    {
                        anim.SetBool("Run", true);
                    }

                    if (isABoss) //BOSS ANIMATIONS
                    {
                        anim.SetBool("Boss Run", true);
                        anim.SetBool("Boss Attack", false);
                    }
                }
            }
            else //WHEN ENEMY CAN'T SEE THE PLAYER
            {
                chasetimer = 0;
                speed = originalSpeed;
                alert.gameObject.SetActive(false);

                if (!isABoss) //CHASER RESET ANIMATIONS
                {
                    anim.SetBool("Start Chasing", false);
                    anim.SetBool("Run", false);
                }

                if (isABoss) //BOSS RESET ANIMATIONS
                {
                    anim.SetBool("Boss Idle", false);
                    anim.SetBool("Boss Run", false);
                    anim.SetBool("Boss Attack", false);
                    anim.SetBool("Boss Walk", true);
                }
            }
        }
        else if (frozenEnemy && isAChaser && hp > 0) //DEACTIVATE ALERT WHEN CHASER IS FROZEN
        {
            alert.gameObject.SetActive(false);
        }
        
        if (!frozenEnemy && isABoss && hp > 0) //BOSS TIMER. CHANGE MECHANICS.
        {
            timerBoss += Time.deltaTime;
            if (timerBoss < 10)
            {
                isAChaser = true;
            }

            if (timerBoss >= 10 && timerBoss < 20)
            {
                isAChaser = false;
                isAShooter = true;
            }

            if (timerBoss >= 20 && timerBoss < 30)
            {
                isAShooter = false;
                isAnInvoker = true;
            }

            if (timerBoss >= 30)
            {
                isAnInvoker = false;
                isAChaser = true;
                timerBoss = 0;
            }

            if (!canSeePlayer && isAnInvoker) //BOSS FLIP WHEN IS A INVOKER 
            {
                flipTimer += Time.deltaTime;
                if (flipTimer <= timeForFlip) { transform.eulerAngles = new Vector3(0, -180, 0); }
                if (flipTimer > timeForFlip) { transform.eulerAngles = new Vector3(0, 0, 0); }
                if (flipTimer >= timeForResetFlip) { flipTimer = 0; }
            }
        }

        if (frozenEnemy && hp > 0 && !isABomb) //WHEN ENEMIES ARE FROZEN
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

        if ((isAShooter || isABoss) && isGrounded && hp <= 0) //WHEN SHOOTER OR BOSS DIE
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            if (bossIsDeath)
            {
                Color color = GetComponent<SpriteRenderer>().material.color;
                color.a -= Time.deltaTime * 0.30f;
                GetComponent<SpriteRenderer>().material.color = color;

                partSystemDoor.transform.position = transform.position;
                finalDoor.transform.position = transform.position;
                partSystemDoor.SetActive(true);
            }
        }

        if (isABoss) //BOSS: BAR HP
        {
            healthBar.fillAmount = hp / maxHealthBoss;
        }

        if (spriteHurt.color == new Color(255, 0, 0, 255)) //HUT ENEMY
        {
            resetHurt += Time.deltaTime;
            if (resetHurt >= 0.15f)
            {
                spriteHurt.color = new Color(255, 255, 255, 255); //ORIGINAL COLOR ENEMY
                resetHurt = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //COLLISION BULLET VS ENEMY
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
                    chaserDie.Play();
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
                shooterDie.Play();
            }

            if (isAnInvoker && hp <= 0 && !isABoss)
            {
                anim.SetBool("Invoker Death", true);
                boxCol1.enabled = false;
                boxCol2.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                invokerDie.Play();
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
                bossDie.Play();
            }
        }        

        //COLLISION ORB VS ENEMY
        if (other.gameObject.tag.Equals("Orb"))
        {
            frozenEnemy = true;
            if (!isABomb)
            {
                frozenSound.Play();
            }

            if (isABomb)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                boxCol1.enabled = false;
                anim.SetBool("Explosion", true);
            }
        }

        //COLLISION ENEMY VS ENEMY OR WALL
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

        //COLLISION VS PLAYER WHEN IS A BOMB
        if (other.gameObject.tag.Equals("Player")  && isABomb)
        {
            hp = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            boxCol1.enabled = false;
            anim.SetBool("Explosion", true);
        }
    }

    //CALLED IN THE ANIMATOR
    void EnemyDie()
    {
        Destroy(gameObject);
    }

    //CALLED IN THE ANIMATOR
    void BossDie()
    {
        partSystemDoor.SetActive(false);
        finalDoor.SetActive(true);
        Destroy(gameObject);
    }

    //CALLED IN THE ANIMATOR
    void ResetAttack()
    {
        if (isAShooter && !isABoss)
        {
            anim.SetBool("Attack", false);
        }

        if (isAnInvoker && !isABoss)
        {
            anim.SetBool("Invoker Attack", false);
        }       
    }

    //PLAYER KNOCKBACK
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

    //FLIP
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

    //DRAW GIZMOS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPlayer.transform.position, lineOfSite);
    }
}
