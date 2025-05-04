using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUI : MonoBehaviour
{

    private void Start()
    {
        endScript.instance.gameEnd += Fade;
        
        gameObject.SetActive(false);

    }

    private void Fade()
    {
        gameObject.SetActive(true);
        StartCoroutine(fadeCo());
    }

    private IEnumerator fadeCo()
    {
        yield return new WaitForSeconds(2f);
        


    }
}
