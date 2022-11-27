using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickmanHandler : MonoBehaviour
{
    public TextAsset monologue;
    public GameObject shutUpButton;
    Coroutine coroutine;
    int _sentenceCounter = 0;
    bool _buttonPressed;
    void Start()
    {
        StartCoroutine(MoveToScene());                
    }

    IEnumerator MoveToScene()
    {
        float targetPos = -3.5f;
        float yPos = -10f;
        float currentTime = 0f, totalTime = 3f;

        while(currentTime < totalTime)
        {
            yPos = Mathf.Lerp(-10f, targetPos, currentTime/totalTime);
            transform.position = new Vector2(transform.position.x, yPos);
            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector2(transform.position.x, targetPos);
        coroutine = StartCoroutine(DisplayMonologue());
    }

    IEnumerator DisplayMonologue()
    {
        Debug.Log("coroutine starts");
        GameObject bubble = transform.GetChild(0).gameObject;
        bubble.SetActive(true);
        Text displayText = GameObject.Find("monologue").GetComponent<Text>();
        //displayText.enabled = true;

        string text = monologue.text;
        var bubbles = text.Split("*");
        string currentSentence;
        
        if(_buttonPressed)
        {   _sentenceCounter = 16;}
        else
        {   _sentenceCounter = 0;}
        

        while(_sentenceCounter < bubbles.Length)
        {
            currentSentence = bubbles[_sentenceCounter];       //this is done unnecessarily every frame but idc.
            displayText.text = currentSentence;

            if(_sentenceCounter == 3)
            {
                if(!shutUpButton.activeSelf)
                {   shutUpButton.SetActive(true);}
            }

            if(_sentenceCounter < 4)
            {
                //Debug.Log("waiting...");
                if(Input.GetMouseButtonDown(0))
                {   _sentenceCounter++;}
            }

            else if(_sentenceCounter >= 4 && _sentenceCounter < 16)
            {
                yield return new WaitForSeconds(8f);
                _sentenceCounter++;
            }

            else if(_sentenceCounter >=16)
            {
                if(shutUpButton.activeSelf)
                {   shutUpButton.SetActive(false);}

                if(Input.GetMouseButtonDown(0))
                {   _sentenceCounter++;}
            }

            //Debug.Log(_sentenceCounter);
            yield return null;
        }

        displayText.enabled = false;
        this.gameObject.SetActive(false);
    }

    public void ShutUp()
    {
        if(_buttonPressed)
        {   return;}

        _buttonPressed = true;
        StopCoroutine(coroutine);
        coroutine = StartCoroutine(DisplayMonologue());
    }
}
