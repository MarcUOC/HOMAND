
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
        //transform.Translate(transform.right * transform.localScale.x * bulletSpeed * Time.deltaTime);
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

    //GOOD
    /* private void OnTriggerEnter2D(Collider2D collision)
     {

         bulletSpeed = 0.15f;
         //collision for destroy orb
         if (collision.gameObject.tag.Equals("Untagged"))
         {
             Destroy(gameObject);
         }
     }*/
    /*private void OnTriggerEnter2D(Collider2D collision)
    {

        bulletSpeed = 0.15f;
        //collision for destroy orb
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            bulletSpeed = 0f;
            Destroy(gameObject, 2);
        }
    }*/

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }*/



    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")){
        bulletSpeed = 0;
        Destroy(gameObject, 1);}
    }*/









}

