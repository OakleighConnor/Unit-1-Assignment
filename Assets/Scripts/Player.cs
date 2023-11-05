using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

    LayerMask groundLayerMask;
    public bool isGrounded = false;
    float speed = 5f;
    HelperScript helper;
    SpriteRenderer spi;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spi = GetComponent<SpriteRenderer>();
        helper = gameObject.AddComponent<HelperScript>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        isGrounded = helper.GroundCheck(isGrounded);
    }

    void Movement()
    {
        //Jump
        if ((Input.GetKeyDown("w") == true) && (isGrounded == true))
        {
            rb.AddForce(new Vector3(0, 16, 0), ForceMode2D.Impulse);
        }

        //Grounded Movement
        if (Input.GetKey("a") == true)
        {
            transform.position = new Vector2(transform.position.x + (-speed * Time.deltaTime), transform.position.y);
        }
        if (Input.GetKey("d") == true)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
    }
}