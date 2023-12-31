using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HelperScript : MonoBehaviour
{
    //Other Components
    SpriteRenderer sr;
    LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
    }
    //FlipObject is used to simply flip the sprites of the enemies and player
    public void FlipObject(bool flip)
    {
        // get the SpriteRenderer component
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        //Flip the sprite calling the method

        if (flip == true)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    public void FlipObjectY(bool flip)
    {
        // get the SpriteRenderer component
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();

        //Flip the sprite calling the method

        if (flip == true)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }
    //GroundCheck checks if the player is grounded
    public bool GroundCheck(bool isGrounded)
    {
        //Raycasting
        Color hitColor = Color.blue;
        float laserLength = 0.1f;
        //Makes 3 lines directly beneath the player to register the floor
        Vector3 rayOffset = new Vector3(0, -0.85f, 0);
        Vector3 rayOffset2 = new Vector3(-0.10f, -0.85f, 0);
        Vector3 rayOffset3 = new Vector3(0.10f, -0.85f, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + rayOffset, Vector2.down, laserLength, groundLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + rayOffset2, Vector2.down, laserLength, groundLayerMask);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + rayOffset3, Vector2.down, laserLength, groundLayerMask);

        //If any of the lines are touching the ground then the player can jump.
        if ((hit.collider != null)||(hit2.collider != null)||(hit3.collider != null))
        {
            hitColor = Color.red;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawRay(transform.position + rayOffset, Vector2.down * laserLength, hitColor);
        Debug.DrawRay(transform.position + rayOffset2, Vector2.down * laserLength, hitColor);
        Debug.DrawRay(transform.position + rayOffset3, Vector2.down * laserLength, hitColor);
        return isGrounded;
    }
}
