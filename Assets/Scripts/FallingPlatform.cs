using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    public float timerForFalling;
    public float timerMaxForFalling;
    public float timerForReset;
    public bool isTriggered;
    
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            timerForFalling += Time.deltaTime;
        }

        if (timerForFalling >= timerMaxForFalling)
        {
            rb.gravityScale = 1;
        }

        if (rb.gravityScale >= 1)
        {
            timerForReset += Time.deltaTime;
            
            if (timerForReset >= 4)
            {
                transform.position = startPos;
                rb.gravityScale = 0;
                timerForFalling = 0;
                timerForReset = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isTriggered = false;
        }        
    }


}
