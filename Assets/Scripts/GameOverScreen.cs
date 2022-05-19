using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public string uploadHighscoreScreen;

    public float timeBetweenShowing, timeToupload = 5f;

    public GameObject score,burdText;

    public Image blackScreen;
    public float blackScreenFade = 2f;
    public Text scoretext;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowObjectsCo());
        StartCoroutine(ShowObjectsCoCo());
        scoretext.text = "SCORE: " +  ScoreController.instance.score.ToString();

        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, blackScreenFade * Time.deltaTime));
    }

    public void Upload()
    {
        SceneManager.LoadScene(uploadHighscoreScreen);
    }

    public IEnumerator ShowObjectsCo()
    {
        yield return new WaitForSeconds(timeBetweenShowing);

        score.SetActive(true);
        yield return new WaitForSeconds(timeBetweenShowing);
        burdText.SetActive(true);
    }

    public IEnumerator ShowObjectsCoCo()
    {
        yield return new WaitForSeconds(timeToupload);

        Upload();
    }
}
