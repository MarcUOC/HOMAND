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
        rb.velocity = transform.right * FireBallSpeed; //FIREBALL MOVEMENT
        Destroy(gameObject, 3);
    }

    //WHEN FIREBALL COLLISION WITH ANYTHING
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("Destroy FireBall", true);
        boxCol.enabled = false;
        FireBallSpeed = 0;
    }

    //CALLED IN THE ANIMATOR
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
