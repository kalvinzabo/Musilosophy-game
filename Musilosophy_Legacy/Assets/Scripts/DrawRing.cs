using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRing : MonoBehaviour   //This code was originally from https://stackoverflow.com/questions/13708395/how-can-i-draw-a-circle-in-unity3d,
//I edited and expanded it to fit my game
{
    LineRenderer lineRenderer;
    [Range(6,60)]   //creates a slider - more than 60 is hard to notice
    public int lineCount;       //more lines = smoother ring
    
    public float width;
    float _currentRadius, _elapsedTime = 0f, _startRadius, _timeToFinishClosing;
    float _circleDuration = 60f/96f;  //lo que dura un beat de la cancion (60/bpm). Lo que pasa es que se adaptará para caer siempre en el segundo en el que tiene que caer.

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.loop = true;
    }
    void OnEnable()
    {
        _currentRadius = _startRadius;
        _elapsedTime = 0f;
        _circleDuration = _timeToFinishClosing - Time.time;
        
    }

    void Draw() //Only need to draw when something changes
    {
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
            gameObject.SetActive(false);
            return;
        }

        if(_elapsedTime > _circleDuration)
        {   
            _currentRadius = 0f;
            return;
        }

        _currentRadius = Mathf.Lerp(_startRadius, 0, _elapsedTime/_circleDuration);
        _elapsedTime += Time.deltaTime;

        Draw();
    }
    
    public void SetPosition(Vector2 position)
    {   this.gameObject.transform.position = position;}

    public void SetRadius(float radius)
    {   _startRadius = radius;}

    public void SetTimeToFinish(float time)
    {   _timeToFinishClosing = time;}
}
