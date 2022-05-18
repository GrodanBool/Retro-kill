using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;

    [HideInInspector] public bool escapedPressed = false;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !escapedPressed)
        {
            PauseUnpause();
            escapedPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && escapedPressed)
        {

        }
    }

    public void PlayerDied()
    {
        // GameObject go = GameObject.Find("Audio Manager");
        // if (go)
        // {
        //     Destroy(go.gameObject);
        // }
        StartCoroutine(PlayerDiedCo());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);

        SceneManager.LoadScene("GameOver");
    }

    public void PauseUnpause()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            escapedPressed = false;
        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
        }
    }
}
