using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float rockSpeed;
    public Rigidbody2D rb;

    void Update()
    {
        //POISON BALL SPEED
        rb.velocity = transform.right * rockSpeed;
        Destroy(gameObject, 4);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
