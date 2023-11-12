using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    //Other components used
    private GameObject player;
    Rigidbody2D rb;

    //Variables
    public float force;
    public float timer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Sets the direction the bullet should be travelling in to go towards the player. Also makes the bullet travel towards the player
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        //If the bullet does not make contact with anything else, then the bullet deletes itself so that there are not loads of versions of it created that never get deleted.
        timer = Time.deltaTime;

        if (timer > 2)
        {
            Destroy(gameObject);
        }
    }

    //Checks if the enemy bullet makes contact with anything else. If it does then it deletes itself to give the idea it shot it.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("shootingEnemy"))
        {
            if (!other.gameObject.CompareTag("smallHealth"))
            {
                if (!other.gameObject.CompareTag("largeHealth"))
                {
                    if (!other.gameObject.CompareTag("smallCoin"))
                    {
                        if (!other.gameObject.CompareTag("largeCoin"))
                        {
                            Destroy(gameObject);
                        }
                    }

                }
            }
        }

        if(other.tag == "Player")
        {
            other.GetComponent<Player>().KBCounter = other.GetComponent<Player>().KBTotalTime;
            other.GetComponent<Player>().KBForce = 3;
            if (other.transform.position.x <= transform.position.x)
            {
                other.GetComponent<Player>().KnockFromRight = true;
            }
            else
            {
                other.GetComponent<Player>().KnockFromRight = false;
            }
            other.GetComponent<Player>().TakeDamage(0.5f);
        }
    }
}
