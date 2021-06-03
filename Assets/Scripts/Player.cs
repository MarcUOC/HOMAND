using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //PLAYER MOVEMENT
    [Header("PLAYER MOVEMENT")]
    public float speed;
    private Rigidbody2D rb;    
    private float moveInput;
    private bool isFacingRight = true;

    //PLAYER JUMP
    [Header("PLAYER JUMP")]
    public float jumpForce;
    private bool doubleJump;
    private bool jumpFalling;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //PLAYER HEALTH & ORB SYSTEM
    [Header("PLAYER HEALTH & ORB SYSTEM")]
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Camera cameraView;
    public Image timerBar;
    public float resetHurt;
    private SpriteRenderer spriteHurt;
    public GameObject triggerBossHP;
    public PauseMenu gamePaused;
    public GameObject backgroundDeath;
    public BoxCollider2D boxcol;

    //PLAYER FIREBALL
    [Header("PLAYER FIREBALL")]    
    public float timeBetweenFireBall;
    public Transform firingPoint;
    public GameObject fireballPrefab;
    public GameObject orbPrefab;
    public bool orbIsOnCooldown;
    public float timerForOrb;
    public float cooldownForOrb;
    [HideInInspector] public float timeUntilFire;

    //PLAYER KNOCKBACK
    [Header("PLAYER KNOCKBACK")]
    [HideInInspector] public float knockback;
    [HideInInspector] public float knockbackLength;
    [HideInInspector] public float knockbackCount;
    [HideInInspector] public bool knockFromRight;

    //PLAYER ANIMATOR
    private Animator anim;
    public AudioSource soundFire;
    public AudioSource soundJump;
    public AudioSource soundOrb;
    public AudioSource soundHurt;
    public GameObject partycleOrbeReady;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteHurt = GetComponent<SpriteRenderer>();
        gamePaused = FindObjectOfType<PauseMenu>();
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (health > 0)
        {
            playerMovement();
            playerDirection();
        }   
    }

    private void Update()
    {
        if (health > 0 && !gamePaused.isPaused)
        {
            //JUMP
            if (Input.GetButtonDown("Jump") && isGrounded == true)
            {
                rb.velocity = Vector2.up * jumpForce;
                doubleJump = true;
                soundJump.Play();
            }

            //DOUBLE JUMP
            if (Input.GetButtonDown("Jump") && isGrounded == false && doubleJump == true)
            {
                rb.velocity = Vector2.up * jumpForce;
                doubleJump = false;
                anim.SetBool("Double Jump", true);
                soundJump.Play();
            }
            else
            {
                anim.SetBool("Double Jump", false);
            }

            //JUMP FALLING
            if (!isGrounded && !jumpFalling)
            {
                jumpFalling = true;
                doubleJump = true;

                if (jumpFalling)
                {
                    if (Input.GetButtonDown("Jump") && doubleJump == true)
                    {
                        rb.velocity = Vector2.up * jumpForce;
                        doubleJump = false;
                        anim.SetBool("Double Jump", true);
                        soundJump.Play();
                    }
                }
            }

            //IF PLAYER GROUNDED
            if (!isGrounded)
            {
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
                jumpFalling = false;
            }

            //THROW FIRE
            if (Input.GetButtonDown("Fire3") && timeUntilFire < Time.time)
            {
                Attack();
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }

            //THROW ORB
            if (Input.GetButtonDown("Fire2") && orbIsOnCooldown == false)
            {
                float angle = isFacingRight ? 0f : 180f;
                Instantiate(orbPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
                orbIsOnCooldown = true;
                anim.SetBool("Attack", true);
                soundOrb.Play();
            }


            //ORB
            if (orbIsOnCooldown)
            {
                partycleOrbeReady.SetActive(false);
                timerForOrb += Time.deltaTime;

                if (timerForOrb >= cooldownForOrb)
                {
                    timerForOrb = 0;
                    orbIsOnCooldown = false;
                }
                timerBar.fillAmount = timerForOrb / cooldownForOrb;
            }
            else
            {
                timerBar.fillAmount = 1;
                partycleOrbeReady.SetActive(true);
            }

            //ORB PARTICLE
            particleOrbPosition();
        }

        //HEARTS
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = fullHeart;
                hearts[i].transform.localScale = new Vector2(0.5f, 0.5f);
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        //HURT
        if (spriteHurt.color == new Color(255, 0, 0, 255)) //color red
        {
            resetHurt += Time.deltaTime;
            if (resetHurt >= 0.15f)
            {
                spriteHurt.color = new Color(255, 255, 255, 255); //color white
                resetHurt = 0;
            }
        }


        //DEATH ANIMATION
        if (health <= 0)
        {
            Time.timeScale = 0.75f;
            cameraView.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            cameraView.orthographicSize = cameraView.orthographicSize - 0.5f * Time.deltaTime;
            anim.SetBool("Death", true);
            backgroundDeath.SetActive(true);
            spriteHurt.sortingOrder = 101;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            boxcol.enabled = false;
        }

        //GOD MODE
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (health <= 7)
            {
                health = health + 1;
            }
            timerForOrb = cooldownForOrb;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                    hearts[i].transform.localScale = new Vector2(1f, 1f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            health = health - 1;           
        }
    }

    //DEAD
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("MovingPlatform"))
        {
            transform.parent = collision.transform;
        }

        if (collision.gameObject.tag.Equals("Final"))
        {
            SceneManager.LoadScene("Final");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("MovingPlatform"))
        {
            transform.parent = null;
        }

        /*if (collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("Trap") || collision.gameObject.tag.Equals("Rock") || collision.gameObject.tag.Equals("Bomb"))
        {
            health = health - 1;
            spriteHurt.color = new Color(255, 0, 0, 255);
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("Trap") || other.gameObject.tag.Equals("Rock") || other.gameObject.tag.Equals("Bomb"))
        {
            health = health - 1;
            soundHurt.Play();
            spriteHurt.color = new Color(255, 0, 0, 255);
        }

        if (other.gameObject.name == "TriggerBossHP")
        {
            triggerBossHP.SetActive(true);
        }
    }

    void particleOrbPosition()
    {
        partycleOrbeReady.transform.position = new Vector3(transform.position.x, transform.position.y + 0.10f, transform.position.z);
    }

    void playerDirection()
    {
        //Player direction
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
            //partycleOrbeReady.transform.position = new Vector3(transform.position.x + 0.20f, transform.position.y, transform.position.z);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
            //partycleOrbeReady.transform.position = new Vector3(transform.position.x - 0.20f, transform.position.y, transform.position.z);
        }

        if (moveInput == 0)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }
    }

    void playerMovement()
    {
        //Player movement and knockback
        if (knockbackCount <= 0)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else
        {
            if (knockFromRight)
            {
                rb.velocity = new Vector2(-knockback, knockback);
            }

            if (!knockFromRight)
            {
                rb.velocity = new Vector2(knockback, knockback);
            }

            knockbackCount -= Time.deltaTime;
            doubleJump = true;
        }
    }

    void Attack()
    {
        float angle = isFacingRight ? 0f : 180f;
        Instantiate(fireballPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        timeUntilFire = Time.time + timeBetweenFireBall;
        soundFire.Play();
    }

    //Call in the animator
    void Die()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
