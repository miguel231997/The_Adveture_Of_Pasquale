using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public float moveSpeed;
    private Animator anim;
    Vector3 localScale;
    public bool movingLeft;
    public bool movingRight;
    Rigidbody2D rb;
    bool isFacingRight;
    transitionManager transitions;

    public int bossHits;
    static int maxBossHits = 9;


    //GroundCheck
    public Transform groundCheck;
    public LayerMask groundLayers;
    private float groundCheckRadius = .2f;
    public bool isGrounded;
    Transform leftWall, rightWall;

    GhostControllerScript ghostPlayer;
    TextBoxManager dialogue;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        dialogue = FindObjectOfType<TextBoxManager>();
        ghostPlayer = FindObjectOfType<GhostControllerScript>();
        transitions = FindObjectOfType<transitionManager>();
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        if(transitions != null && transitions.currentScene == 6)
        {
            leftWall = GameObject.Find("LeftWall").GetComponent<Transform>();
            rightWall = GameObject.Find("RightWall").GetComponent<Transform>();
        }

        bossHits = 0;

        //Animations stuff
        anim = GetComponent<Animator>();
        anim.SetBool("isIdle", true);

        movingRight = false;
        movingLeft = false;



        isGrounded = true;

    }

    // Update is called once per frame
    void Update()
    {

       

        
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        anim.SetBool("isGrounded", isGrounded);


        if (movingLeft)
        {
            moveLeft();
            anim.SetBool("isIdle", false);
        }
            
        else if (movingRight)
        {
            moveRight();
            anim.SetBool("isIdle", false);
        }
            
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = this.transform.localScale;
        playerScale.x *= -1;
        this.transform.localScale = playerScale;
    }

    void moveLeft()
    {

        localScale.x = -1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }

    void moveRight()
    {

        localScale.x = 1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerScene2")
        {
            
            movingRight = false;
            movingLeft = true;

            anim.SetBool("isIdle", true);
            anim.SetTrigger("bossIdle");

        }
        
        
        if (gameObject != null)
            {
            //collision logic here
            if (collision.gameObject.tag == "bullet")
                {
                  //audio.PlayOneShot(enemySound);
                  //Destroy.collision(gameObject);
                  //Destroy.gameObject;
                  Destroy(collision.gameObject);

                }
            }

        if (gameObject != null)
        {
            //collision logic here
            if (collision.gameObject.tag == "energyBall")
            {
                Destroy(collision.gameObject);
                if (bossHits < maxBossHits)
                {
                    bossHits++;
                }

                else 
                {

                    AudioManager.Instance.backgroundMusic3.Stop();
                    AudioManager.Instance.backgroundMusic2.Play();
                    ghostPlayer.allowMoving = false;
                    dialogue.ReloadScript(transitions.afterBossDies);
                    dialogue.EnableTextBox();
                    Destroy(gameObject);
                    transitions.bossKilled = true;
                }


            }
        }

        if(collision.gameObject.tag =="Player")
        {
            if(movingLeft == true)
            {
                movingLeft = false;
                movingRight = true;
            }

            else 
            {
                movingLeft = true;
                movingRight = false;
            }
        }


        if (collision.gameObject.tag == "RightWallBossFight")
        {
            movingLeft = true;
            movingRight = false;
        }

        if (collision.gameObject.tag == "LeftWallBossFight")
        {
            movingLeft = false;
            movingRight = true;
        }
    }
}
