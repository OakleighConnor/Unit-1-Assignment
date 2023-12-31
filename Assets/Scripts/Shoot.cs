using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public Transform Gun;

    Vector2 direction;

    public GameObject Bullet;

    public float BulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    float ReadyForNextShot;

    public GameObject Player;
    Player deathCheck;

    //Sounds
    [SerializeField] private AudioSource shootSFX;

    // Start is called before the first frame update
    void Start()
    {
        deathCheck = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Gun.position;
        FaceMouse();

        //Shooting Script. Only activates if the player isn't dead (obviously)
        if (!deathCheck.dead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time > ReadyForNextShot)
                {
                    ReadyForNextShot = Time.time + 0.2f / fireRate;
                    shoot();
                }
            }
        }

    }

    void FaceMouse() //Pointing Gun Towards Cursor
    {
        Gun.transform.right = direction;
    }

    void shoot() //Shooting
    {
        shootSFX.Play();
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * BulletSpeed);
        Destroy(BulletIns, 3);
    }
}
