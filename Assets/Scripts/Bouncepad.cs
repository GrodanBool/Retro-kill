using UnityEngine;

public class Bouncepad : MonoBehaviour
{   
    public float bounceforce;

   void OnTriggerEnter(Collider other)
   {
        PlayerController.instance.Bounce(bounceforce);
        AudioManagerMusicSFX.instance.PlaySFX(6);
   }
}
