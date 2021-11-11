using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        //collision logic here
        if (collision.gameObject.tag == "energyBall")
        {
            //audio.PlayOneShot(enemySound);
            //Destroy.collision(gameObject);
            //Destroy.gameObject;
            Destroy(collision.gameObject);

        }
    }
}
