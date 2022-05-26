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
    private bool uploadSuccess;

    void Start()
    {
        score.text = "SCORE  " + ScoreController.instance.score.ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (iField.text.Length > 0 && !uploadSuccess)
        {
            uploadButton.GetComponent<Button>().interactable = true;
            Color selected = uploadButton.GetComponentInChildren<Text>().color;
            selected.a = 1f;
            uploadButton.GetComponentInChildren<Text>().color = selected;

        }
        else
        {
            uploadButton.GetComponent<Button>().interactable = false;
            Color selected = uploadButton.GetComponentInChildren<Text>().color;
            selected.a = 0.5f;
            uploadButton.GetComponentInChildren<Text>().color = selected;
        }
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
            uploadSuccess = true;
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