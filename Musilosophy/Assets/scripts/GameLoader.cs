using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public GameObject scoresScreen, fetchingText, scoresText;
    bool _scoresDisplaying, _justExited;
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(_justExited)
            {
                _scoresDisplaying = false;
                _justExited = false;
                return;
            }
            if(_scoresDisplaying)
            {   return;}

            SceneManager.LoadScene("GameScene");
        }
    }

    public void DisplayScores()
    {
        _scoresDisplaying = true;

        scoresScreen.SetActive(true);
        StartCoroutine(Fetching());
    }

    public void BackToMenu()
    {
        _justExited = true;

        scoresScreen.SetActive(false);
    }

    IEnumerator Fetching()
    {
        yield return new WaitForSeconds(2f);
        fetchingText.SetActive(false);
        scoresText.SetActive(true);        
    }

    
}
