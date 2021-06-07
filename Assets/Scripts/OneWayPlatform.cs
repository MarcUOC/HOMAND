using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effect;
    //public float delay;
    public bool playerTouch;

    void Start()
    {
        effect = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        //CHANGE PLATFORM COLLISION
        if (playerTouch == true)
        {
            if ((Input.GetButtonDown("Vertical")) || (Input.GetAxisRaw("Vertical") < -0.5f))
            {
                effect.rotationalOffset = 180f;
            }
        }

        //RESET WHEN PLAYER JUMP
        if (Input.GetButton("Jump"))
        {
            effect.rotationalOffset = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            playerTouch = true;
        } 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            playerTouch = false;
        }
    }
}
