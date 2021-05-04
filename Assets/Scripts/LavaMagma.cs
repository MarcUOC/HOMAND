using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMagma : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxcol;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxcol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        boxcol.enabled = false;
        anim.SetBool("LavaMagma Collision", true);

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

    void whenDestroyed()
    {
        Destroy(gameObject);
    }
}


