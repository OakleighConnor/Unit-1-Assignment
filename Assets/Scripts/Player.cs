using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Variables
    public bool isGrounded = false;
    float speed = 5f;
    public bool dead = false;
    public int respawn;
    public Vector3 respawnPoint;

    //Knockback Variables
    [HideInInspector] public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    [HideInInspector] public bool KnockFromRight;

    //Sounds
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource damageSFX;
    [SerializeField] private AudioSource coinSSFX;
    [SerializeField] private AudioSource coinLSFX;
    [SerializeField] private AudioSource heartSSFX;
    [SerializeField] private AudioSource heartLSFX;
    [SerializeField] private AudioSource death;

    //Other Components used
    LayerMask groundLayerMask;
    HelperScript helper;
    SpriteRenderer spi;
    Rigidbody2D rb;
    Animator anim;
    public GameObject Gun;
    public GameObject Crosair;
    public Text CoinText;
    public GameObject GameOver;

    //Health and Coins:
    public float startingHealth;
    public float currentHealth { get; private set; }
    public float coins;


    // Start is called before the first frame update

    private void Awake()
    {
        currentHealth = startingHealth;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spi = GetComponent<SpriteRenderer>();
        helper = gameObject.AddComponent<HelperScript>();
        groundLayerMask = LayerMask.GetMask("Ground");

        respawnPoint = transform.position;
        coins = 00;
        CoinText.text = ":" + coins;
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = helper.GroundCheck(isGrounded);
        //Methods inside this script
        //Make sure AnimationReset is above Movement else animations break
        AnimationReset();
        if (!dead)
        {
            if (KBCounter <= 0)
            {
                //Activates when the player is alive
                Movement();

                //Makes the gun appear and point towards the cursor. Also makes the crosair become the cursor. Turns the base cursor off
                Gun.SetActive(true);
            }
            else
            {
                //Disables the gun while taking knockback
                Gun.SetActive(false);
                if (KnockFromRight)
                {
                    rb.velocity = new Vector2(-KBForce, KBForce);
                }
                else
                {
                    rb.velocity = new Vector2(KBForce, KBForce);
                }
                
                KBCounter -= Time.deltaTime;
            }

            Crosair.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            //Makes the gun and crosair disappear for the death animation. Also makes the cursor visible during this time so the player can still see where it is.
            Gun.SetActive(false);
            Crosair.SetActive(false);
            Cursor.visible = true;
            GameOver.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(respawn);
            }
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            damageSFX.Play();
        }
        else
        {
            if (!dead)
            {
                death.Play();
                anim.SetTrigger("die");
                dead = true;
            }
        }
    }
    private void AnimationReset()
    {
        anim.SetBool("walking", false);
        if (isGrounded)
        {
            anim.SetBool("falling", false);
        }

        //Points the player towards the direction the gun is pointing
        if (Crosair.transform.position.x <= transform.position.x)
        {
            helper.FlipObject(true);
        }
        else
        {
            helper.FlipObject(false);
        }
    }

    private void Movement()
    {
        //Jump
        if ((Input.GetKeyDown("w") == true) && (helper.GroundCheck(isGrounded)))
        {
            jumpSoundEffect.Play();
            anim.SetBool("falling", true);
            rb.AddForce(new Vector3(0, 21, 0), ForceMode2D.Impulse);
        }

        //Grounded movement left and right
        if (Input.GetKey("a") == true)
        {
            if (isGrounded)
            {
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }
            transform.position = new Vector2(transform.position.x + (-speed * Time.deltaTime), transform.position.y);
        }
        if (Input.GetKey("d") == true)
        {
            if (isGrounded)
            {
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
    }

    //Adds the collected item to the total
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    public void AddCoins(float _value)
    {
        coins = coins + _value;
        CoinText.text = ":" + coins;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("largeCoin"))
        {
            coinLSFX.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("smallHealth"))
        {
            heartSSFX.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("largeHealth"))
        {
            heartLSFX.Play();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("smallCoin"))
        {
            coinSSFX.Play();
            Destroy(collision.gameObject);
        }
    }
}