using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private bool movingRight = true;
    public int hp;

    public Rigidbody2D rb;
    public Transform groundDetection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //enemy movement
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f);

        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //collision bullet vs enemy
        if (other.gameObject.tag.Equals("Bullet"))
        {
            hp--; //Enemy -1 hp
            if (hp <= 0)
            {
                Destroy(gameObject); //Enemy die
            }
        }

        //collision player vs enemy
        if (other.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("Game"); //Player die
        }

        /*if (other.gameObject.tag.Equals("Orb"))
        {
            speed = 0;
        }*/
    }

    void OnCollisionExit2D(Collision2D other)
     {
         speed = 1;
     }

    //When orb exit from trigger...
    private void OnTriggerExit2D(Collider2D collision)
    {
        speed = 1;
    }

    //When orb stay in trigger...
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Orb"))
        {
            speed = 0;
        }
    }







}
