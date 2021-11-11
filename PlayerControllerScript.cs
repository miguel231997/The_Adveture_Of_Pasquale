     using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : MonoBehaviour
{

    public float speed = 200f;
    public float jumpForce = 300f;
    private float xSpeed;
    public float acceleration, deceleration, maxSpeed;
    public bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayers;
    private float groundCheckRadius = .2f;
    public bool isFacingRight, idle;
    bool jump;
    public AudioClip enemySound, collectibleSound;
    public bool canMove;
    

    public transitionManager transitions;
    private Animator anim;
    int playerLayer;
    int enemyLayer;
    bool coroutineAllowed = true;
    Color color;
    Renderer rend;
    public GameObject BulletToLeft, BulletToRight;
    Vector2 bulletPos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;

    // Use this for initialization
    void Start()
    {
        playerLayer = this.gameObject.layer;
        enemyLayer = LayerMask.NameToLayer("enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        //initialize your stuffs here.
        maxSpeed = 3;
        xSpeed = 0;
        acceleration = 10;
        deceleration = 10;
        anim = GetComponent<Animator>();
        isFacingRight = true;
        idle = true;
        anim.SetBool("isIdle", true);
        transitions = FindObjectOfType<transitionManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(!canMove)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            anim.SetTrigger("humanShooting");
            nextFire = Time.time + fireRate;
            fire();
        }

    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        anim.SetBool("isGrounded", isGrounded);
        if (!canMove)
        {
            return;
        }
        if (jump)
        {
            Debug.Log("I AM JUMPING NOW! WHEEEEEE!!!");
            if (isGrounded)
            {
                Debug.Log("ACTUALLY JUMPIONG PHYSICS");
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            }
            else
            {
                jump = false;
            }
        }
        float moveX = Input.GetAxis("Horizontal");
        if ((Input.GetKey(KeyCode.A)) && (xSpeed > -maxSpeed))
            xSpeed = xSpeed - acceleration * Time.deltaTime;
        else if ((Input.GetKey(KeyCode.D)) && (xSpeed < maxSpeed))
            xSpeed = xSpeed + acceleration * Time.deltaTime;
        else
        {
            if (xSpeed > deceleration * Time.deltaTime)
                xSpeed = xSpeed - deceleration * Time.deltaTime;
            else if (xSpeed < -deceleration * Time.deltaTime)
                xSpeed = xSpeed + deceleration * Time.deltaTime;
            else
                xSpeed = 0;
        }
        Vector2 moving = new Vector2(xSpeed, GetComponent<Rigidbody2D>().velocity.y);
        if (moveX != 0f)
        {
            idle = false;
        }
        else
        {
            idle = true;
        }
        anim.SetBool("isIdle", idle);
        if ((moveX > 0.0f && !isFacingRight) || (moveX < 0.0f && isFacingRight))
        {
            Flip();
        }
        GetComponent<Rigidbody2D>().velocity = moving;


    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = this.transform.localScale;
        playerScale.x *= -1;
        this.transform.localScale = playerScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            anim.SetTrigger("humangettinghit");
            HealthBarScript.health -= 10f;
            
            if (coroutineAllowed)
                StartCoroutine("Immortal");
        }

        if(collision.gameObject.tag == "doorInHouse")
        {

 
            Invoke("Scene1toScene2", transitions.delayTime);

        }

        if (collision.gameObject.tag == "BossScene2")
        {
            anim.SetTrigger("humangettinghit");
            anim.SetTrigger("humanDead");
            
            Invoke("DelayedAction", transitions.delayGhostWorld);


        }
    }

    void DelayedAction()
    {
        AudioManager.Instance.backgroundMusic2.Stop();
        SceneManager.LoadScene(3);
        AudioManager.Instance.backgroundMusic3.Play();
    }

    void Scene1toScene2()
    {
        AudioManager.Instance.backgroundMusic.Stop();
        SceneManager.LoadScene(2);
        AudioManager.Instance.backgroundMusic2.Play();
    }

    IEnumerator Immortal()
    {
        coroutineAllowed = false;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        color.a = .5f;
        rend.material.color = color;
        yield return new WaitForSeconds(3f);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        color.a = 1f;
        rend.material.color = color;
        coroutineAllowed = true;
    }

    void fire()
    {
        if (canMove)
        {
            bulletPos = transform.position;
            if (isFacingRight)
            {
                bulletPos += new Vector2(+1f, +.03f);
                Instantiate(BulletToRight, bulletPos, Quaternion.identity);
            }
            else
            {
                bulletPos += new Vector2(-1f, +.03f);
                Instantiate(BulletToLeft, bulletPos, Quaternion.identity);
            }
        }
    }
}
