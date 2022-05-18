using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncepad : MonoBehaviour
{   
    public float bounceforce;

   void OnTriggerEnter(Collider other)
   {
       if (other.tag == "Player")
       {
           PlayerController.instance.Bounce(bounceforce);
           AudioManagerMusicSFX.instance.PlaySFX(6);
       }
   }
}
