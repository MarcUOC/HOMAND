using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    float timeUntilFire;
    public bool orbIsOnCooldown;
    public float timerForOrb;
    public float cooldownForOrb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    
    void FixedUpdate()
    {
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
            transform.localScale = new Vector3(1f, 1, 1f);
            isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1, 1f);
            isFacingRight = false;
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
        }

        //DOUBLE JUMP
        if (Input.GetButtonDown("Jump") && isGrounded == false && doubleJump == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            doubleJump = false;
        }

        
        //THROW ARROWS
        if (Input.GetButtonDown("Fire3") && timeUntilFire < Time.time)
        {
            float angle = isFacingRight ? 0f : 180f;
            Instantiate(arrowPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            timeUntilFire = Time.time + timeBetweenArrow;
        }


        //THROW ORB
        if (Input.GetButtonDown("Fire2") && orbIsOnCooldown == false)
        {
            float angle = isFacingRight ? 0f : 180f;
            Instantiate(orbPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            orbIsOnCooldown = true;
        }

        if (orbIsOnCooldown == true)
        {
            timerForOrb += Time.deltaTime;
            if (timerForOrb >= cooldownForOrb)
            {
                timerForOrb = 0;
                orbIsOnCooldown = false;
            }            
        }
    }

    //DEAD
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Die"))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
