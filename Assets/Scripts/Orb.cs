
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public Rigidbody2D rb;
    Enemy enemy;

    private void Start()
    {
      
    }

    private void Update()
    {
        //direction orb
        rb.velocity = transform.right * bulletSpeed;        
        Destroy(gameObject, 4);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Untagged"))
        {
            Destroy(gameObject);            
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    









}

