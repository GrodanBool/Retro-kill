using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    float templateHeight = 20f;
    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = transform.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(entryContainer, entryTemplate);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);
        }
    }
}
