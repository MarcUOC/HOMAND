
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public Rigidbody2D rb;
    //Player pm;
    Enemy enemyMove;

    private void Start()
    {
        //pm = gameObject.GetComponent<Player>();
        //enemyMove = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
        //direction orb
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, 7);
        //transform.Translate(transform.right * transform.localScale.x * bulletSpeed * Time.deltaTime);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Untagged"))
        {
            Destroy(gameObject);
            
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            bulletSpeed = 0;
            //enemyMove.speed = 0;
            Destroy(gameObject, 3);
        }
    }*/

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bulletSpeed = 0.15f;
        //collision for destroy orb
        if (collision.gameObject.tag.Equals("Untagged"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }



    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")){
        bulletSpeed = 0;
        Destroy(gameObject, 1);}
    }*/









}

