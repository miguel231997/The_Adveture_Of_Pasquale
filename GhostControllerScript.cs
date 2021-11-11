using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GhostControllerScript : MonoBehaviour
{

    public float speed = 200f;
    public float jumpForce = 300f;
    private float xSpeed;
    public float acceleration, deceleration, maxSpeed;
    public bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayers;
    private float groundCheckRadius = .2f;
    private bool isFacingRight, idle;
    bool jump;
    private AudioSource audio;
    public AudioClip enemySound, collectibleSound;
    public bool allowMoving;

    private Animator anim;
    int playerLayer;
    int enemyLayer;
    bool coroutineAllowed = true;
    Color color;
    Renderer rend;

    public GameObject EnergyBallLeft, EnergyBallRight, gameOverText, restartButton;
    Vector2 ballPos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;
    transitionManager transitions;
    BossScript boss;

    // Use this for initialization
    void Start()
    {
        boss = FindObjectOfType<BossScript>();
        allowMoving = false;
        if (gameOverText != null)
            allowMoving = true;
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        transitions = FindObjectOfType<transitionManager>();
        //initialize your stuffs here.
        playerLayer = this.gameObject.layer;
        enemyLayer = LayerMask.NameToLayer("enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        maxSpeed = 3;
        xSpeed = 0;
        acceleration = 10;
        deceleration = 10;
        anim = GetComponent<Animator>();
        isFacingRight = true;
        idle = true;
        anim.SetBool("isIdle", true);
        audio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!allowMoving)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            fire();
        }
        
    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        anim.SetBool("isGrounded", isGrounded);
        if(!allowMoving)
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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 playerScale = this.transform.localScale;
        playerScale.x *= -1;
        this.transform.localScale = playerScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "traps")
        {
            anim.SetTrigger("ghostGettingHit");
            HealthBarScript.health -= 15f;
            if (HealthBarScript.health <= 0)
            {
                gameOverText.SetActive(true);
                restartButton.SetActive(true);
                gameObject.SetActive(false);
                StopCoroutine("Immortal");
                // yield return new WaitForSeconds(10.0f);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //StartCoroutine("restart");
                return;
            }
            
            else if (coroutineAllowed)
                StartCoroutine("Immortal");
        }

        if(collision.gameObject.tag == "Boss")
        {
            anim.SetTrigger("ghostGettingHit");
            HealthBarScript.health -= 50f;
            if (HealthBarScript.health == 0)
            {
                gameOverText.SetActive(true);
                restartButton.SetActive(true);
                gameObject.SetActive(false);
                StopCoroutine("Immortal");
                allowMoving = true;
                boss.movingLeft = true;
                // yield return new WaitForSeconds(10.0f);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //StartCoroutine("restart");
                return;
            }

            else if (coroutineAllowed)
                StartCoroutine("Immortal");
        }

        if (collision.gameObject.tag == "doorInTower")
        {
            Invoke("DelayedAction2", transitions.delayTime);
        }

        if(collision.gameObject.tag == "doorInTower2")
        {
            SceneManager.LoadScene(5);
        }
    }
    //IEnumerator restart()
    //{
    //    yield return new WaitForSeconds(10.0f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
    void DelayedAction2()
    {
        SceneManager.LoadScene(4);
    }



    IEnumerator Immortal()
    {
        coroutineAllowed = false;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        color.a = .5f;
        rend.material.color = color;
        yield return new WaitForSeconds(2.5f);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        color.a = 1f;
        rend.material.color = color;
        coroutineAllowed = true;
    }

    void fire()
    {
        if (allowMoving)
        {
            ballPos = transform.position;
            if (isFacingRight)
            {
                ballPos += new Vector2(+1f, +.03f);
                Instantiate(EnergyBallRight, ballPos, Quaternion.identity);
            }
            else
            {
                ballPos += new Vector2(-1f, +.03f);
                Instantiate(EnergyBallLeft, ballPos, Quaternion.identity);
            }
        }
    }
}
