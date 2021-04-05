using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float moveInput;
    public bool isFacingRight = true;

    public float jumpForce;
    public bool doubleJump;
    
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    
    void FixedUpdate()
    {
        //Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //Player movement
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
