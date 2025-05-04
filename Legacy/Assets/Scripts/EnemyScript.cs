using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{


    //Movement 
    protected Rigidbody2D rb;
    [Header("Movement")]
    public float speed = 2f;
    public Transform playerTransform;
    protected float hDirection = 1;
    protected bool canMove = true;
    //Sprite & Animation 
    public SpriteRenderer sprite;
    [SerializeField]
    public Animator animationController;
    [SerializeField]
    private GameObject hitEffect;
    float raycastStartPos;
    public LayerMask groundLayer;

    //Attacking
    [Header("Attacking")]
    public float attackRange = 2f;
    public Collider2D hitbox;
    public GameObject playerObject;
    protected Health healthManager;
    protected bool active = false;
    private bool invincible = false;
    private float invincibilityFrames = 0.5f;

    //Sounds
    [Header("Sounds")]
    public AudioClip dieSound;
    public AudioClip damageSound;
    public AudioClip awakeSound;


    public float IFrames { get { return invincibilityFrames; } set { invincibilityFrames = value; } }
    public bool Invincible { get { return invincible; } }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<Health>();
        hitEffect.gameObject.SetActive(false);
        raycastStartPos = sprite.bounds.size.y / 64;
        active = true;

    }


    protected  void MoveTowards(Vector2 targetPosition) {
        if (!canMove) return;
        if (DistToTarget(targetPosition) < attackRange) {
            return;
        }
        hDirection = rb.position.x < targetPosition.x ? 1 : -1;
        if (rb.position.x < targetPosition.x) {
            sprite.flipX = true;
            rb.velocity = new Vector2(speed, rb.velocity.y);
        } else {
            sprite.flipX = false;
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }




    }

    protected float DistToTarget(Vector2 targetPosition) {
        float distToTarget = Mathf.Sqrt((targetPosition.x - rb.position.x) * (targetPosition.x - rb.position.x) + (targetPosition.y - rb.position.y) * (targetPosition.y - rb.position.y));
        return distToTarget;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!active) return;
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            if (playerObject.GetComponent<PlayerScript>().Invincible) return;
            playerObject.GetComponent<PlayerScript>().IFrames = 0.5f;
            playerObject.GetComponent<PlayerScript>().onHit(hDirection);
            playerObject.GetComponent<Health>().takeDamage(healthManager.Damage);
        }
        
        if (collision.gameObject.CompareTag("PlayerAttack")) {
            if(invincible) return;
            hitEffect.SetActive(true);
            StartCoroutine(hitEffectCo());
            knockBack();
        }

        if (collision.gameObject.CompareTag("Spikes")){
            healthManager.takeDamage(100);
        }
    }
    protected IEnumerator hitEffectCo() {
        if (invincible) yield break;
        canMove = false;
        soundFXManager.instance.PlaySoundFXClip(damageSound, transform,0.4f, UnityEngine.Random.Range(1f, 1.2f));
        yield return new WaitForSeconds(0.45f);
        hitEffect.gameObject.SetActive(false);
        canMove = true;
        if (healthManager.Dead) yield break;
        invincible = true;
        sprite.color = new Color(255f, 225f, 255f, 0.2f);
        yield return new WaitForSeconds(invincibilityFrames);
        invincible = false;
        sprite.color = Color.white;
    }
    protected IEnumerator DeathCouroutine() {
        canMove =false;
        rb.velocity = Vector2.zero;
        soundFXManager.instance.PlaySoundFXClip(dieSound, transform, 0.4f, UnityEngine.Random.Range(1f, 1.2f));
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);    
    }

    protected void knockBack() { 
        
        rb.velocity = new Vector2(playerObject.GetComponent<PlayerScript>().HDirection * 5f, rb.velocity.y);  
    }

    protected bool isGrounded()
    {
        return Physics2D.Raycast(new Vector2(rb.position.x, rb.position.y - raycastStartPos), -Vector2.up, 0.7f,groundLayer);
      
    }

}
 