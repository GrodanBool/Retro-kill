using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UploadHighScore : MonoBehaviour
{
    public string returnTo;
    public float timeBeforeClosing = 5f;
    public InputField iField;
    public Text score;
    public GameObject fail;
    public GameObject success;
    public GameObject uploading;
    public GameObject uploadButton;

    void Start()
    {
        score.text = "SCORE  " + ScoreController.instance.score.ToString();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnTo()
    {
        SceneManager.LoadScene(returnTo);
        PlayerPrefs.SetString("CurrentLevel", "");
    }

    public async void UploadScore()
    {
        uploading.SetActive(true);
        try
        {
            await ScoreManager.instance.UploadNewHighScore(iField.text, ScoreController.instance.score);
        }
        catch
        {

        }

        if (ScoreManager.instance.uploadSuccess)
        {
            uploadButton.GetComponent<Button>().interactable = false;
            Color selected = uploadButton.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            uploadButton.GetComponentInChildren<Text>().color = selected;

            uploading.SetActive(false);
            success.SetActive(true);
            Invoke("HideSuccess", timeBeforeClosing);
        }
        else
        {
            uploading.SetActive(false);
            fail.SetActive(true);
            Invoke("HideFail", timeBeforeClosing);
        }
    }

    public void HideSuccess()
    {
        success.SetActive(false);
    }
    public void HideFail()
    {
        fail.SetActive(false);
    }
}