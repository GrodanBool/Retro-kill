using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    public string lvl1, lvl2, returnTo;
    public Image story;
    public Text storyText;
    public Text countdownText;
    public float countdown;
    private bool fadeInComplete = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (story.gameObject.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (story.color.a <= 1)
            {
                story.color = new Color(story.color.r, story.color.g, story.color.b, Mathf.MoveTowards(story.color.a, 1f, 1 * Time.deltaTime));
                if (story.color.a >= 1 && !fadeInComplete)
                {
                    storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, Mathf.MoveTowards(storyText.color.a, 1f, 1 * Time.deltaTime));
                    countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, Mathf.MoveTowards(countdownText.color.a, 1f, 1 * Time.deltaTime));
                    if (storyText.color.a >= 1 && countdownText.color.a >= 1)
                    {
                        fadeInComplete = true;
                        Debug.Log(fadeInComplete);
                    }
                }
            }

            if (countdown > 0f)
            {
                countdown -= Time.deltaTime;
                countdownText.text = Convert.ToInt32(countdown).ToString();
            }

            if (countdown < 2f && fadeInComplete)
            {
                storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, Mathf.MoveTowards(storyText.color.a, 0, 1 * Time.deltaTime));
                countdownText.color = new Color(countdownText.color.r, countdownText.color.g, countdownText.color.b, Mathf.MoveTowards(countdownText.color.a, 0, 1 * Time.deltaTime));
                AudioManager.instance.StopBGM();
            }

        }
    }
    public void Level1()
    {
        story.gameObject.SetActive(true);
        Invoke("LoadLevel1", countdown);
    }
    public void Level2()
    {
        AudioManager.instance.StopBGM();
        SceneManager.LoadScene(lvl2);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }

    public void LoadLevel1()
    {
        AudioManager.instance.StopBGM();
        SceneManager.LoadScene(lvl1);
        PlayerPrefs.SetString("CurrentLevel", "");
    }
}
