using UnityEngine;
using UnityEngine.SceneManagement;

public class Bouncepad : MonoBehaviour
{   
    public float bounceforce;

   void OnTriggerEnter(Collider other)
   {
       if (other.tag == "Player" && SceneManager.GetActiveScene().name != "OnlineLevel")
       {
           PlayerController.instance.Bounce(bounceforce);
           AudioManagerMusicSFX.instance.PlaySFX(6);
       }
       else if (other.tag == "Player" && SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            PlayerOnlineController.instance.Bounce(bounceforce);
            AudioManagerMusicSFX.instance.PlaySFX(6);
        }
   }
}
