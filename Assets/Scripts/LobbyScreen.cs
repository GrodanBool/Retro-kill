using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScreen : MonoBehaviour
{
    public string returnTo;

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
}