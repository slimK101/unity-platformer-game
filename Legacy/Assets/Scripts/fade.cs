using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{

    public Image fadeImage;
    // Start is called before the first frame update
    private void Start()
    {
        endScript.instance.gameEnd += Fade;
        fadeImage.color = new Color(0f,0f,0f,0f);
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Fade() {
        gameObject.SetActive(true);
        StartCoroutine(fadeCo());
    }

    private IEnumerator fadeCo() {
        yield return new WaitForSeconds(2f);
        float time = 0f;
        float alpha = 0f;

        while (time < 1f) {
            alpha += 0.05f; 
            fadeImage.color = new Color(0f, 0f,0f,alpha);
            time += Time.deltaTime;
            yield return null;  
        }


        
    }
}
