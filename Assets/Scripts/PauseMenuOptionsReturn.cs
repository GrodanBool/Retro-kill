using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOptionsReturn : MonoBehaviour
{
    public GameObject options, pause;

    public void ReturnClicked()
    {
        if (options.activeInHierarchy)
        {
            options.SetActive(false);
            pause.SetActive(true);
        }
    }

    public void OptionsClicked()
    {
        if (pause.activeInHierarchy)
        {
            options.SetActive(true);
            pause.SetActive(false);
        }
    }
}
