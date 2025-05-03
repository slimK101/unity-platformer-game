using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int health;
    [Header("Stats")]
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int damage;
    private bool dead = false;

    public healthBar HealthBar;


    //Propreties
    public bool Dead { get {return dead; } set { dead = value; } }
    public int Damage { get { return damage; }}
    private void Awake()
    {
        health = maxHealth;
    }

    public void takeDamage(int damage) { 
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (HealthBar == null) return;
        HealthBar.takeDamage(damage);

    }

    private void Update()
    {


        if (health == 0)
        {
            dead = true;

        }
    }

    

}
