using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, ammoText,score,activeModifiers,multiplier;

    // public Image damageEffect;
    // public float damageAlpha = .25f, damageFadeSpeed = 2f;

    public GameObject pauseScreen;

    
    // public float fadeSpeed = 1.5f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "SCORE: " +  ScoreController.instance.score.ToString();
        multiplier.text =  ScoreController.instance.totalMultiplier.ToString() + "x";
        // if(damageEffect.color.a != 0)
        // {
        //     damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        // }
    }

    // public void ShowDamage()
    // {
    //     damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
    // }
}
