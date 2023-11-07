using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sr;
    SpriteRenderer funSprite;

    public Transform Gun;

    public GameObject gunobj;

    Vector2 direction;

    public GameObject Bullet;

    public float BulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    float ReadyForNextShot;

    LayerMask groundLayerMask;
    HelperScript helper;
    float speed = 2f;
    public int health;
    private bool right = false;
    public bool attacking = false;
    public GameObject player;
    float AggroRange = 5;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        funSprite = GetComponentInChildren<SpriteRenderer>();
        groundLayerMask = LayerMask.GetMask("Ground");
        helper = gameObject.AddComponent<HelperScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("shootingEnemy"))
        {
            attacking = PlayerCheck(attacking);
        }

        //Methods in script
        if (!attacking)
        {
            Movement();
        }
        else
        {
            Attack();
        }

        //Methods in HelperScript
        helper.HealthCheck(health);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            health = health - 1;
            Destroy(other.gameObject);
        }

    }

    void Movement()
    {
        //Raycasting
        Color hitColor = Color.blue;
        float laserLength = 0.1f;
        //2 Lines below the enemy to register if there is nothing in the direction they are travelling
        Vector3 rayOffsetDL = new Vector3(-0.21f, -0.51f, 0);
        Vector3 rayOffsetDR = new Vector3(0.21f, -0.51f, 0);
        RaycastHit2D hitDL = Physics2D.Raycast(transform.position + rayOffsetDL, Vector2.down, laserLength, groundLayerMask);
        RaycastHit2D hitDR = Physics2D.Raycast(transform.position + rayOffsetDR, Vector2.down, laserLength, groundLayerMask);
        //2 Lines to the side of the enemy to 
        Vector3 rayOffsetLeft = new Vector3(-0.45f, 0, 0);
        Vector3 rayOffsetRight = new Vector3(0.45f, 0, 0);
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



        if (right)
        {
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + (-speed * Time.deltaTime), transform.position.y);
        }
    }

    bool PlayerCheck(bool attacking)
    {
        float seperation = Vector3.Distance(this.transform.position, player.transform.position);
        print(seperation);
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

    void Attack()
    {
        //Vector2 playerPos = (player.transform.position);
        //direction = playerPos - (Vector2)Gun.position;

        //Gun.transform.right = direction;

        Vector3 relativePos = player.transform.position - Gun.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = Gun.transform.rotation.x;
        rotation.y = Gun.transform.rotation.y;
        Gun.transform.rotation = rotation;
        if (player.transform.position.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1);
        }


        timer += Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (timer > 0.5f)
        {
            timer = 0;
            shoot();
        }
    }
    void shoot()
    {
        Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
    }
}