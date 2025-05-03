using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundFXManager : MonoBehaviour
{
    // Start is called before the first frame update
   public static soundFXManager instance;
    [SerializeField]
    private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null) { 
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume,float pitch) { 
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position,Quaternion.identity);  

        audioSource.clip = audioClip;   

        audioSource.volume = volume;    

        audioSource.pitch = pitch;
        audioSource.Play(); 

        float clipLength = audioSource.clip.length;   

        Destroy(audioSource.gameObject,clipLength);
    
    }


   }

