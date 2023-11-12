using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private AudioSource clash;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.CompareTag("enemyProj"))
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
            else
            {
                if (other.gameObject.CompareTag("enemyProj"))
                {
                    clash.Play();
                }
            }
        }
    }
}
