using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadHighScore : MonoBehaviour
{
    public string returnTo;
    public InputField iField;
    public Text score;

    private void Start()
    {
        score.text = "SCORE: " + ScoreController.instance.score.ToString();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }

    public void UploadScore()
    {
        ScoreManager.instance.SaveScore(iField.text, ScoreController.instance.score);
    }
}