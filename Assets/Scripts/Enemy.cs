using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private bool movingRight = true;
    public int hp;
    public bool frozenEnemy;
    public float frozenTime;
    public float frozenMaxTime;
    public Rigidbody2D rb;
    public Transform groundDetection;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //enemy movement
        if (!frozenEnemy)
        { 
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

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

        if (frozenEnemy == true)
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
        if (other.gameObject.tag.Equals("Bullet"))
        {
            hp--; //Enemy -1 hp
            if (hp <= 0)
            {
                Destroy(gameObject); //Enemy die
            }
        }


        //collision player vs enemy
        /*if (other.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("Game"); //Player die
        }*/


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
}
