using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float bulletSpeed = 15f;
    public Rigidbody2D rb;

    private void Update()
    {
        //Assign velocity to arrow
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
