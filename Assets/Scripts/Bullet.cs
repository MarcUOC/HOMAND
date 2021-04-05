using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 15f;
    public Rigidbody2D rb;

    private void Update()
    {
        //Assign velocity to bullet
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, 2);
        //transform.Translate(transform.right * transform.localScale.x * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    
}
