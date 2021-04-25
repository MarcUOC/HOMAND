using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Player movement
    private Rigidbody2D rb;
    public float speed;
    private float moveInput;
    public bool isFacingRight = true;

    //Player Jump
    public float jumpForce;
    public bool doubleJump;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //Player Knockback
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    //Player shoot
    public float timeBetweenArrow = 0.2f;
    public Transform firingPoint;
    public GameObject arrowPrefab;
    public GameObject orbPrefab;
    public float timeUntilFire;
    public bool orbIsOnCooldown;
    public float timerForOrb;
    public float cooldownForOrb;

    //ANIMATOR
    public Animator anim;

    //HEALTH SYSTEM
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Camera cameraView;

    public Image timerBar;
    public float timeLeft;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    
    void FixedUpdate()
    {
        /*if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / cooldownForOrb;
        }*/

        //HUD: FILL BAR ORB
        /*if (orbIsOnCooldown)
        {
            timeLeft += Time.deltaTime;
            //timerBar.fillAmount = 0;
            orbIsOnCooldown = true;
            timerBar.fillAmount = timeLeft / cooldownForOrb;
        }
        else
        {
            timerBar.fillAmount = 1;
            orbIsOnCooldown = false;
            //timerBar.fillAmount = timeLeft / cooldownForOrb;
            timeLeft = 0;
        }*/

        


        //HUD: HEARTS
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = fullHeart;
                hearts[i].transform.localScale = new Vector2(0.4f, 0.4f);
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

        if (health <= 0)
        {
            Time.timeScale = 0.5f;
            cameraView.orthographicSize = cameraView.orthographicSize - 1f * Time.deltaTime;
            anim.SetBool("Death", true);
        }






        moveInput = Input.GetAxisRaw("Horizontal");

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
        

        //Player direction
        if (moveInput > 0)
        {
            //transform.localScale = new Vector3(1f, 1, 1f);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            //transform.localScale = new Vector3(-1f, 1, 1f);
            transform.eulerAngles = new Vector3(0, -180, 0);
            isFacingRight = false;
        }

        if (moveInput == 0)
        {
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }


        //Player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void Update()
    {
        //JUMP
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            doubleJump = true;
            //anim.SetBool("Jump", true);
        }
        //else { anim.SetBool("Jump", false); }


        //DOUBLE JUMP
        if (Input.GetButtonDown("Jump") && isGrounded == false && doubleJump == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            doubleJump = false;
            anim.SetBool("Double Jump", true);
        }
        else { anim.SetBool("Double Jump", false); }

        if (!isGrounded)
        { 
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);
        }


        /*if (!isGrounded && doubleJump && (Input.GetButtonDown("Jump")))
        {
            anim.SetBool("Double Jump", true);
        }
        else
        {
            anim.SetBool("Double Jump", false);
        }*/

        //THROW ARROWS
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
        }

        if (orbIsOnCooldown)
        {
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
            //orbIsOnCooldown = false;
            //timerBar.fillAmount = timeLeft / cooldownForOrb;
            //timerForOrb = 0;
        }

        
    }

    //DEAD
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Die"))
        {
            SceneManager.LoadScene("Game");
        }

        if (collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("Rock"))
        {
            //anim.SetBool("Hurt", true);
            health = health - 1;
        }
        //else { anim.SetBool("Hurt", false); }
    }

    void Attack()
    {
        float angle = isFacingRight ? 0f : 180f;
        Instantiate(arrowPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        timeUntilFire = Time.time + timeBetweenArrow;
    }

    void Die()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
