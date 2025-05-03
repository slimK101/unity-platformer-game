using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : EnemyScript
{

    private float wakeRange = 4f;
    private void Start()
    {
        active = false;
        sprite.gameObject.SetActive(false);
    }



    void Update()
    {
        if (!active && DistToTarget(playerTransform.position) < wakeRange) {
            StartCoroutine(wakeSkeleton());
        
        }
        if (!active) { return; }


        Vector2 playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
        if (playerObject.GetComponent<PlayerScript>().Active) MoveTowards(playerPosition);



        if (healthManager.Dead)
        {
            animationController.SetTrigger("deathTrig");

            active = false;
            StartCoroutine(DeathCouroutine());
            return;
        }


        
    }
    private IEnumerator wakeSkeleton() {
        sprite.gameObject.SetActive(true);
        soundFXManager.instance.PlaySoundFXClip(awakeSound, transform, 0.7f, UnityEngine.Random.Range(1f, 1.2f));
        canMove = false;
        active = true;
        yield return new WaitForSeconds(0.9f);
        canMove = true;
      
        animationController.SetTrigger("walk");
    
    }

}