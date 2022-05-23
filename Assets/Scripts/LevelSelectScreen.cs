using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    public string lvl1, lvl2, returnTo;
    public Image story;
    public Text storyText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (story.gameObject.activeInHierarchy)
        {
            story.color = new Color(story.color.r, story.color.g, story.color.b, Mathf.MoveTowards(story.color.a, 1f, 1 * Time.deltaTime));
            if (story.color.a >= 1)
            {
                storyText.color = new Color(storyText.color.r, storyText.color.g, storyText.color.b, Mathf.MoveTowards(storyText.color.a, 100f, 1 * Time.deltaTime));
            }
        }
    }
    public void Level1()
    {
        story.gameObject.SetActive(true);
        Invoke("LoadLevel1", 10);
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
