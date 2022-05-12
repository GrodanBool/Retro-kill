using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorTime : MonoBehaviour
{

    public Renderer engineBodyRenderer;
    public float speed;
    public Color startColor, endColor;
    float startTime;


    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(ChangeEngineColour());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ChangeEngineColour()
{
    float tick = 0f;
    while (engineBodyRenderer.material.color != endColor)
    {
        //Debug.Log("color");
        tick += Time.deltaTime * speed;
        engineBodyRenderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1));
        yield return null;
    }
}
}
