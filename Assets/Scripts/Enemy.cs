using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Other components used
    SpriteRenderer sr;
    LayerMask groundLayerMask;
    HelperScript helper;
    Animator anim;
    Rigidbody2D rb;

    //Other objects refered to in the enemy
    public Transform Gun;
    public GameObject gunobj;
    public GameObject Bullet;
    public Transform ShootPoint;
    public GameObject player;
    public Player playerScript;

    //Variables used for the enemy
    Vector2 direction;
    float speed = 2f;
    public int health;
    private bool right = false;
    float timer;

    float KBCounter = 0;
    float KBForce = 4;
    bool KnockFromRight;
    //These variables are only used in the shooting enemy, they are only refered to if the enemy has the "shootingEnemy" tag
    public float BulletSpeed;
    public float fireRate;
    public bool attacking = false;
    float AggroRange = 9;

    //Sounds
    [SerializeField] private AudioSource damage;
    [SerializeField] private AudioSource die;
    [SerializeField] private AudioSource shooting;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        groundLayerMask = LayerMask.GetMask("Ground");
        helper = gameObject.AddComponent<HelperScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (KBCounter <= 0)
        {
            if (gameObject.CompareTag("shootingEnemy"))
            {
                attacking = PlayerCheck(attacking);
                anim.SetBool("attacking", false);
                //PlayerCheck checks if the player is within the AggroRange
                if (PlayerCheck(attacking))
                {
                    //Attack makes the enemy point a gun towards and shoot at the player. 
                    Attack();
                }
            }

            //Checks if the enemy is a shooting enemy that has a player within the AggroRange. If so the enemy stops walking to focus on shooting towards the player
            if (!attacking)
            {
                Movement();
                transform.localScale = new Vector3(1.3f, 1.3f, 1);
            }

            //Checks if the enemy is dead or not
            if (health == 0)
            {
                die.Play();
                Destroy(gameObject);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, KBForce);
            KBCounter -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If the enemy is hit by a player projectile, then they take damage. When they run out of health, they die. 
        if (other.gameObject.CompareTag("Projectile"))
        {
            health = health - 1;
            if (other.transform.position.x <= transform.position.x)
            {
                KnockFromRight = true;
            }
            else
            {
                KnockFromRight = false;
            }
            KBCounter = 0.1f;
            damage.Play();
            Destroy(other.gameObject);
        }

    }
    void Movement()
    {
        //Raycasting
        Color hitColor = Color.blue;
        float laserLength = 0.1f;
        //2 Lines below the enemy to register if there is nothing in the direction they are travelling
        Vector3 rayOffsetDL = new Vector3(-0.21f, -0.85f, 0);
        Vector3 rayOffsetDR = new Vector3(0.21f, -0.85f, 0);
        RaycastHit2D hitDL = Physics2D.Raycast(transform.position + rayOffsetDL, Vector2.down, laserLength, groundLayerMask);
        RaycastHit2D hitDR = Physics2D.Raycast(transform.position + rayOffsetDR, Vector2.down, laserLength, groundLayerMask);
        //2 Lines to the side of the enemy to 
        Vector3 rayOffsetLeft = new Vector3(-0.45f, -0.75f, 0);
        Vector3 rayOffsetRight = new Vector3(0.45f, -0.75f, 0);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + rayOffsetLeft, Vector2.left, laserLength, groundLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + rayOffsetRight, Vector2.right, laserLength, groundLayerMask);

        //Checks if the raycast cannot sense the ground. If this is the case the enemy turns around.
        if ((hitDL.collider == null) || (hitLeft.collider != null))
        {
            hitColor = Color.red;
            right = true;
        }
        if ((hitDR.collider == null) || (hitRight.collider != null))
        {
            hitColor = Color.red;
            right = false;
        }
        //Checks if the raycast cannot sense the ground. If this is the case the enemy turns around.


        Debug.DrawRay(transform.position + rayOffsetDL, Vector2.down * laserLength, hitColor);
        Debug.DrawRay(transform.position + rayOffsetDR, Vector2.down * laserLength, hitColor);
        Debug.DrawRay(transform.position + rayOffsetLeft, Vector2.left * laserLength, hitColor);
        Debug.DrawRay(transform.position + rayOffsetRight, Vector2.right * laserLength, hitColor);

        //This code checks the direction the enemy should be walking in. It also checks the tag on the enemy to see how fast they should be moving in a direction

        if (right)
        {
            if (gameObject.CompareTag("fastEnemy"))
            {
                transform.position = new Vector2(transform.position.x + (6 * Time.deltaTime), transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
            }
            helper.FlipObject(false);
        }
        else
        {
            if (gameObject.CompareTag("fastEnemy"))
            {
                transform.position = new Vector2(transform.position.x + (-4 * Time.deltaTime), transform.position.y); ;
            }
            else
            {
                transform.position = new Vector2(transform.position.x + (-speed * Time.deltaTime), transform.position.y);
            }
            helper.FlipObject(true);
        }
    }

    //Checks if the player is within the AggroRange of the enemy
    bool PlayerCheck(bool attacking)
    {
        float seperation = Vector3.Distance(this.transform.position, player.transform.position);
        if (seperation <= AggroRange)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
        return attacking;
    }

    //Makes the enemy shoot towards the player. This code should only ever be activated by the shooting enemies. Not anyone else.
    void Attack()
    {
        anim.SetBool("attacking", true);

        //Points the gun towards the player
        Vector3 relativePos = player.transform.position - Gun.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = Gun.transform.rotation.x;
        rotation.y = Gun.transform.rotation.y;
        Gun.transform.rotation = rotation;
        //Checks the player's position in comparison to the enemy. Flips the enemy towards the direction of the player
        if (player.transform.position.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }


        timer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (timer > 0.5f)
        {
            timer = 0;
            shoot();
        }
    }

    //Creates an enemy bullet. The enemy bullet has its own script to move towards the player
    void shoot()
    {
        Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
        shooting.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerScript.KBCounter = playerScript.KBTotalTime;
            playerScript.KBForce = 6;
            if (other.transform.position.x <= transform.position.x)
            {
                playerScript.KnockFromRight = true;
            }
            else
            {
                playerScript.KnockFromRight = false;
            }
            playerScript.TakeDamage(1f);
        }
    }

    public void ResetEnemies()
    {
        gameObject.SetActive(true);
    }
}