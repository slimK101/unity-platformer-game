using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    [SerializeField]
    public float delay = 0f;
    public GameObject spikes;
    private Rigidbody2D rb;
    private float initialPos;
    private float height = 2f;
    public GameObject playerObj;
    public AudioClip upSound;
    // Start is called before the first frame update
    void Awake()
    {

        rb = spikes.GetComponent<Rigidbody2D>();
        initialPos = rb.position.y;
        

        StartCoroutine(After(movementcoroutine(), delay));


    }


    private IEnumerator After(IEnumerator coroutine, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return coroutine;
    }


    private IEnumerator movementcoroutine() {

        //Spikes Start going up 
        Vector2 startPos = rb.position;
        Vector2 targetPos = new Vector2(rb.position.x, initialPos + height);
        float time = 0.1f;
        float elapsed = 0f;
       //soundFXManager.instance.PlaySoundFXClip(upSound, transform, 0.7f, 1f);
        while (elapsed < time)
        {
            //rb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);    
            yield return null;

        }


        rb.MovePosition(targetPos);

        yield return new WaitForSeconds(1f);
        //Spikes start going down
        startPos = rb.position; 
        targetPos = new Vector2(rb.position.x, initialPos);
        time = 0.2f;
        elapsed = 0f;
        while (elapsed < time)
        {
            //rb.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);
            yield return null;

        }
        yield return new WaitForSeconds(1f);    

        StartCoroutine(movementcoroutine());



    }

    
}
