using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScreen : MonoBehaviour
{
    public string returnTo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void GfxLevel()
    {
        Debug.Log("High maybe");

    }

    public void Language()
    {
        Debug.Log("You chose english");

    }

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);

        PlayerPrefs.SetString("CurrentLevel", "");

    }
}
