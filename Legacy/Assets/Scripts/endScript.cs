using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScript : MonoBehaviour
{

    public static endScript instance;


    public delegate void GameEnd();
    public event GameEnd gameEnd;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

  
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

        if (collision.gameObject.CompareTag("Player")) { 
        
            gameEnd.Invoke();
        }
    }
}
