using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHurtbox : MonoBehaviour
{
    public Collider2D hurtbox;
    public float offset = 0;
    public Health healthManager;
    [SerializeField]
    private string targetTag = "Enemy";
    private bool active = false;
    
    public bool Active { get { return active; } set { active = value; } }


    private void Awake()
    {
        hurtbox = GetComponent<Collider2D>();
        hurtbox.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hurtbox.enabled = active;
        hurtbox.offset = new Vector2(offset,hurtbox.offset.y);    
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(targetTag+"Hitbox")){
            if (collider.GetComponentInParent<EnemyScript>().Invincible) return;
            collider.GetComponentInParent<Health>().takeDamage(healthManager.Damage);
     

        }
    
    }

    

}
