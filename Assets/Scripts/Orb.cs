
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float orbSpeed = 5f;
    public Rigidbody2D rb;

    private void Update()
    {
        //ORB SPEED
        rb.velocity = transform.right * orbSpeed;        
        Destroy(gameObject, 4);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //CHECK COLLISIONS
        if (collision.gameObject.tag.Equals("Untagged") || collision.gameObject.tag.Equals("Enemy") || collision.gameObject.tag.Equals("Wall") || 
            collision.gameObject.tag.Equals("Bomb") || collision.gameObject.tag.Equals("Trap") || collision.gameObject.tag.Equals("MovingPlatform"))
        {
            Destroy(gameObject);
        }
    }

        

    









}

