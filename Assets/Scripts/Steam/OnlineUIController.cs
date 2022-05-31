using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class OnlineUIController : NetworkBehaviour
{
    public static OnlineUIController instance;

    public Slider healthSlider;
    public Text healthText, ammoText, score, activeModifiers, multiplier;
    public GameObject pauseScreen;
    public Image fadeIn;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, Mathf.MoveTowards(fadeIn.color.a, 0, 1 * Time.deltaTime));

        //OnlineUIController.instance.score.text = "SCORE: " + ScoreController.instance.score.ToString();
        //OnlineUIController.instance.multiplier.text = ScoreController.instance.totalMultiplier.ToString() + "x";
    }
}
