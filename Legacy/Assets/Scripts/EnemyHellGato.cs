using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHellGato : EnemyScript
{
    

    private float jumpDistance = 5f;
    private bool started = true;
    [SerializeField]
   
    // Start is called before the first frame update


    


    private void Update()
    {
        if (started && DistToTarget(playerTransform.position) < 15f) {
            StartCoroutine("moveRoutine");
            started = false;
        }
            
        if (active && healthManager.Dead) {
            StopCoroutine("moveRoutine");
            animationController.SetTrigger("deathTrig");
            active = false;
            StartCoroutine(DeathCouroutine());
        }

    }


    public IEnumerator moveRoutine() {
        //yield return new WaitForSeconds(0f);
        //keeps looking for a target
        while (DistToTarget(playerObject.transform.position) > Random.Range(jumpDistance,jumpDistance + 2f)) {
            if (isGrounded())
            {
                MoveTowards(playerObject.transform.position);
            }
            yield return null;
        }
        //Jumps at target
        jump();
        
        float t = 0f;
        while (t < 0.1f) {
            rb.AddForce(new Vector2(hDirection * 7f, 0));
            t += Time.deltaTime;    
            yield return null;
        }
       

        rb.velocity.Set(rb.velocity.x, 0);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(moveRoutine());  
    }

    private void jump() {
        if (!isGrounded()) return;
        if (!healthManager.Dead) {
            animationController.SetTrigger("jump");
        }
        
        rb.velocity = new Vector2(rb.velocity.x, Random.Range(5f, 6f));
    
    }

  
}
