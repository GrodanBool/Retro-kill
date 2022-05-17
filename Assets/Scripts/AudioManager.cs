using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource menuMusic;

    private GameObject[] other;
     private bool NotFirst = false;

    public void Awake()
    {

        other = GameObject.FindGameObjectsWithTag("Music");

        foreach (GameObject oneOther in other)
         {
             if (oneOther.scene.buildIndex == -1)
             {
                 NotFirst = true;
             }
         }

         if (NotFirst == true)
         {
             Destroy(gameObject);
         }
         DontDestroyOnLoad(transform.gameObject);
         menuMusic = GetComponent<AudioSource>();

         instance = this;
     }
 
        
    


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

     public void PlayMenuMusic()
     {
          if (menuMusic.isPlaying) return;
         menuMusic.Play();
     }

    public void StopBGM()
    {
        menuMusic.Stop();
          Destroy(gameObject);
    }
}
