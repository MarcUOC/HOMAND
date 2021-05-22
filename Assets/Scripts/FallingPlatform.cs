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
    public GameObject particleSys;
    public BoxCollider2D boxCol1;
    public BoxCollider2D boxCol2;
    
    public Vector3 startPos;

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
            particleSys.SetActive(true);

            if (timerForFalling >= timerMaxForFalling)
            {
                rb.gravityScale = 0.5f;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
        else
        {
            particleSys.SetActive(false);
        }

        if (rb.gravityScale >= 0.5f)
        {
            timerForReset += Time.deltaTime;

            if (timerForReset >= 0.3f)
            {
                boxCol1.enabled = false;
                boxCol2.enabled = false;
            }

            if (timerForReset >= 2.5f)
            {                
                transform.position = startPos;
                rb.gravityScale = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                timerForReset = 0;
                timerForFalling = 0;
                boxCol1.enabled = true;
                boxCol2.enabled = true;
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
