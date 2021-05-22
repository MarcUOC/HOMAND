using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float FireBallSpeed;
    public Rigidbody2D rb;
    public Animator anim;
    private BoxCollider2D boxCol;

    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        rb.velocity = transform.right * FireBallSpeed;
        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("Destroy FireBall", true);
        boxCol.enabled = false;
        FireBallSpeed = 0;
    }

    //Call in the animator Destroy FireBall.
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
