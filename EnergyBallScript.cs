using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallScript : MonoBehaviour
{
    public float velX = 5f;
    float velY = 0f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
        Destroy(gameObject, 1.5f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject != null)
        {
            //collision logic here
            if (collision.gameObject.tag == "enemy")
            {
                //audio.PlayOneShot(enemySound);
                //Destroy.collision(gameObject);
                //Destroy.gameObject;
                Destroy(collision.gameObject);

            }
        }
    }
}
