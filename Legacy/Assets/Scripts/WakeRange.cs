using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeRange : MonoBehaviour
{

    private bool isEnemy;

    public bool EnemyInRange { get { return isEnemy; } }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnemy = true;
            StartCoroutine(GetComponentInParent<EnemyHellGato>().moveRoutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnemy = true;
            
        }
    }
}
