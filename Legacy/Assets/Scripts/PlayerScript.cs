using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerScript : MonoBehaviour
{
    #region Variables 
    //Movement Variables
    private bool canMove = true;
    private float horizontalInput = 0f;
    private bool inAir;
    private bool canJump;
    public Rigidbody2D rb;
    private float hDirection = 1f;
    [Header("Movement")]
    public float speed = 5f;
    public float jumpHeight = 7f;
    public LayerMask groundLayer;
    private float jumpHold;



    //Physics & Animation
    [Header("Physics")]
    [Header("Animation")]
    private Sprite gameSprite;
    public Animator animationController;
    public SpriteRenderer spriteRenderer;
    private float raycastStartPos = 0f;
    private bool onLandBuffer = false;
    private bool runStarted = false;
    

    //Attack Variables
    private bool canAttack = true;
    private bool isAttacking = false;
    private float attackCooldown = 0.3f;
    private bool pushed = false;
    private float pushDirection = 1;
    private bool active;
    private Health healthManager;
    public AttackHurtbox attackHurtbox;
    private bool invincible = false;
    private float invincibilityFrames = 0.5f;
    private bool attackKeyPressed = false;

    //Sound Clips
    [Header("Sounds")]
    public AudioClip attackSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip runSound;
    public AudioClip[] hits;
    public AudioClip dieSound;

    //Propreties
    public bool Active { get { return active; } }
    public bool Pushed { get { return active; } }
    public bool Invincible { get { return invincible; } }
    public float HDirection { get { return hDirection; } }
    public float IFrames { get { return invincibilityFrames; } set { invincibilityFrames = value; } }




    string currentSceneName;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<Health>();
        attackHurtbox = GetComponentInChildren<AttackHurtbox>();
        currentSceneName = SceneManager.GetActiveScene().name;
        raycastStartPos = spriteRenderer.bounds.size.y / 64;

        endScript.instance.gameEnd += playerStop;
        active = true;



    }

    private void Start()
    {

    }

    void Update()
    {
        if (!active) return;


        if (!healthManager.Dead)
        {
            getInput();
            jump();
            move();
            PushBack();
            animate();
            
        }
        else {
            pushed = false;
            rb.velocity = new Vector2 (0, rb.velocity.y);
            animationController.SetTrigger("deathTrig");
            soundFXManager.instance.PlaySoundFXClip(dieSound, transform, 1f, UnityEngine.Random.Range(1f, 1.2f));
            StartCoroutine(death());
            animate();
            active = false;

        }
      
        
       
        

    }

    public void reloadScene() {
        SceneManager.LoadScene(currentSceneName);

    }
    public void doExitGame()
    {
        Application.Quit();
    }


    private IEnumerator death() {
        yield return new WaitForSeconds(1f);
        reloadScene();  
    }


    private void getInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        canJump = isGrounded() && Input.GetKey(KeyCode.Space) && !isAttacking;

      
        if (!attackKeyPressed && Input.GetKey(KeyCode.G))
        {
            attackKeyPressed = true;
            handleAttack();
            
        }
        if(attackKeyPressed) attackKeyPressed = !Input.GetKeyUp(KeyCode.G);

    }

    private void move()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else {
           
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
        
    }

    private void animate()
    {
        inAir = !isGrounded() && Mathf.Abs(rb.velocity.y) > 0;
        if (!isAttacking && canMove)
        {
            if (horizontalInput == 1f) hDirection = 1f; else if (horizontalInput == -1f) hDirection = -1f ;

            if (hDirection == 1f)
            {
                spriteRenderer.flipX = false;
                attackHurtbox.offset = 0.8f;
            }

            else {
                spriteRenderer.flipX = true;
                attackHurtbox.offset = - 0.88f;
               
            }

            

        }
        if (isGrounded())
        {
            if (Mathf.Abs(rb.velocity.x) > 0)
            {
                animationController.SetBool("isRunning", true);


            }
            else
            {
                animationController.SetBool("isRunning", false);

            }
        }
        animationController.SetBool("inAir", inAir);
        animationController.SetBool("isAttacking", isAttacking);
        animationController.SetBool("isPushed", pushed);
        
    
    }

  
    
    private bool isGrounded()
    {
        return Physics2D.Raycast(new Vector2(rb.position.x , rb.position.y - raycastStartPos), -Vector2.up, 0.7f, groundLayer);
       
    }

   

    private void jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            soundFXManager.instance.PlaySoundFXClip(jumpSound, transform, 0.7f, UnityEngine.Random.Range(0.7f, 1.5f));
            onLandBuffer = true;//inused 
        }
    }


    private void handleAttack()
    {

        if (!canAttack)
        {
            return;
        }

        if (inAir)
        {
            return;
        }
        canMove = false;
        isAttacking = true;
        
        StartCoroutine(StartAttackCooldown());

    }

    private IEnumerator StartAttackCooldown()
    {
        
        attackHurtbox.Active = true;    
        canAttack = false;
        soundFXManager.instance.PlaySoundFXClip(attackSound, transform, 0.7f,UnityEngine.Random.Range(1f,1.5f));
        yield return new WaitForSeconds(attackCooldown);
        canMove = true;
        attackHurtbox.Active = false;
        isAttacking = false;
        yield return new WaitForSeconds(0.1f);//Cooldown between each attack 
        canAttack = true;
        
       




    }

    public void onHit(float enemyDirection) {
        if (pushed || healthManager.Dead) {
            return;
        }
        if (invincible) return;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        soundFXManager.instance.PlaySoundFXClip(hits[UnityEngine.Random.Range(0, 4)], transform, 1f, UnityEngine.Random.Range(1f, 1.2f));
        StartCoroutine(PushBackForce(enemyDirection));
    }

    private IEnumerator PushBackForce(float enemyDirection) {
        canMove = false;
        pushed = true;
        pushDirection =  enemyDirection;
        hDirection = -enemyDirection;
        if (healthManager.Dead) yield break;
        spriteRenderer.color = Color.red;
        invincible = true;
        yield return new WaitForSeconds(.4f);
        canMove = true;
        pushed = false;
        
        spriteRenderer.color = new Color(255f,255f, 255f,0.2f);
        yield return new WaitForSeconds(invincibilityFrames);
        invincible = false;
        spriteRenderer.color = Color.white;
    }

    private void PushBack() {
        if (!pushed ) return;
        if (healthManager.Dead) return;
        rb.AddForce(new Vector2(pushDirection* 4f ,0),ForceMode2D.Impulse);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes") && gameObject.CompareTag("Player")) {
            if (invincible) return;
            IFrames = 0.7f;
          
            onHit(-HDirection);  
            healthManager.takeDamage(collision.gameObject.GetComponent<Health>().Damage);
            
           
           
        }
    }

    public void playerStop() {
        active = false;

    
    }

}



