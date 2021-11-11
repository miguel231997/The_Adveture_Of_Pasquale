using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{

    public float moveSpeed = 2f;
    Transform leftWall, rightWall;
    Vector3 localScale;
    bool movingLeft = true;
    Rigidbody2D rb;
  
 

    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        leftWall = GameObject.Find("leftWall").GetComponent<Transform>();
        rightWall = GameObject.Find("rightWall").GetComponent<Transform>();

    }

    void Update()
    {

        if (transform.position.x > rightWall.position.x)
            movingLeft = true;
        if (transform.position.x < leftWall.position.x)
            movingLeft = false;
        if (movingLeft)
            moveLeft();
        else
            moveRight();
    }

    void moveLeft()
    {
        movingLeft = true;
        localScale.x = -1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }

    void moveRight()
    {
        movingLeft = false;
        localScale.x = 1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }
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