using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawRing : MonoBehaviour   //This code was originally from https://stackoverflow.com/questions/13708395/how-can-i-draw-a-circle-in-unity3d,
//I edited and expanded it to fit my game
{
    LineRenderer lineRenderer;
    [Range(6,60)]   //creates a slider - more than 60 is hard to notice
    public int lineCount;       //more lines = smoother ring
    
    public float width;
    float _currentRadius, _elapsedTime = 0f, _startRadius;
    float _circleDuration = 60f/96f;  //lo que dura un beat de la cancion (60/bpm). Lo que pasa es que se adaptará para caer siempre en el segundo en el que tiene que caer.
    bool _hasClicked = false;
    Text scoreText;
    Color _randomColor;

    static float score = 0f;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.loop = true;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }
    void OnEnable()
    {
        _hasClicked = false;
        _currentRadius = _startRadius;
        _elapsedTime = 0f;     //quiza haya aqui parte del problema, que esto no tiene en cuenta que la cancion no empieza en Time.time == 0. Al pasarle nextBeat*segundos, le pasas el momento en el que tocaria ese beat si la cancion hubiera empezado en 0. Lo ideal seria pasarle esa cantidad mas el retraso en empezar. O que aqui en vez de hacer el calculo con time.time lo hiciera con el tiempo del otro... Habria que pasarle directamente por parametro la duracion del circulo, y no el momento de acabar.
        //Debug.Log("el ratio de la duracion del circulo sobre un beat es de: " + _circleDuration/(60f/96f));
        _randomColor = new Color(Random.Range(0.2f,1f), Random.Range(0.2f,1f), Random.Range(0.2f,1f), 1f);
    }

    void Draw() //Only need to draw when something changes
    {
        lineRenderer.startColor = _randomColor;
        lineRenderer.endColor = _randomColor;
        lineRenderer.positionCount = lineCount;
        lineRenderer.startWidth = width;
        float theta = (2f * Mathf.PI) / lineCount;  //find radians per segment
        float angle = 0;
        for (int i = 0; i < lineCount; i++)
        {
            float x = _currentRadius * Mathf.Cos(angle);
            float y = _currentRadius * Mathf.Sin(angle);
            
            lineRenderer.SetPosition(i, new Vector2(x, y));
            //switch 0 and y for 2D games
            angle += theta;
        }
    }

    void Update()
    {
        if(_currentRadius == 0f)
        {
            ChangeScore();
            gameObject.SetActive(false);
            return;
        }

        if(Input.GetMouseButtonDown(0) && !_hasClicked)
        {   _hasClicked = true;}

        if(_elapsedTime >= _circleDuration)
        {   
            _currentRadius = 0f;
            return;
        }

        _currentRadius = Mathf.Lerp(_startRadius, 0, _elapsedTime/_circleDuration);
        _elapsedTime += Time.deltaTime;

        Draw();
    }

    void ChangeScore()
    {
        if(!_hasClicked)
        {   return;}

        if(score > 10000)
        {   score += Random.Range(-270, 950);}
        else
        {   score += Random.Range(120, 950);}

        if(Random.Range(1,201) == 1)
        {   
            scoreText.text = "SCORE: SIKE";
            return;
        }

        scoreText.text = ("SCORE: " + score);
    }
    
    public void SetPosition(Vector2 position)
    {   this.gameObject.transform.position = position;}

    public void SetRadius(float radius)
    {   _startRadius = radius;}

    public void SetDuration(float duration)
    {   _circleDuration = duration;}
}
