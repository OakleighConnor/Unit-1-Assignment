using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    float healthValue;
    float value;
    float type;
    public bool reload;

    //Sounds

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("smallHealth"))
        {
            healthValue = 0.5f;
            type = 1;
        }
        if (gameObject.CompareTag("largeHealth"))
        {
            healthValue = 1f;
            type = 1;
        }
        if (gameObject.CompareTag("smallCoin"))
        {
            value = 1f;
            type = 2;
        }
        if (gameObject.CompareTag("largeCoin"))
        {
            value = 10f;
            type = 2;
        }
        /*if (gameObject.CompareTag("reset"))
        {
            type = 3;
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(type == 1)
            {
                collision.GetComponent<Player>().AddHealth(healthValue);
            }
            else if (type == 2)
            {
                collision.GetComponent<Player>().AddCoins(value);
            }
            else if (type == 3)
            {
                collision.GetComponent<Player>().respawnPoint = transform.position;
            }
        }
    }
}
