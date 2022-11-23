using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public float minCircleDuration, maxCircleDuration, minRadius, maxRadius;
    float _SCREENWIDTH, _WIDTHOFFSET, _SCREENHEIGHT, _HEIGHTOFFSET;
    Vector2 _SCREENCORNER, _randomPos;  //The corner is the origin, bottom left.

    bool nextCircleReady = true;
    Camera myCamera;
    void Start()
    {
        myCamera = UnityEngine.Camera.main;

        _SCREENHEIGHT = myCamera.orthographicSize*2;
        _SCREENWIDTH = _SCREENHEIGHT*myCamera.aspect;
        _WIDTHOFFSET = _SCREENWIDTH/8;
        _HEIGHTOFFSET = _SCREENHEIGHT/8;
        _SCREENCORNER = Vector2.zero - new Vector2(_SCREENWIDTH/2, _SCREENHEIGHT/2);
    }
    void Update()
    {
        if(!nextCircleReady)
        {   return;}

        _randomPos = _SCREENCORNER + new Vector2(Random.Range(
            _WIDTHOFFSET, _SCREENWIDTH - _WIDTHOFFSET),     //x position of the new circle
            Random.Range(
            _HEIGHTOFFSET, _SCREENHEIGHT - _HEIGHTOFFSET));     //y position of the new circle

        StartCoroutine(SpawnPoint(_randomPos, Random.Range(minRadius, maxRadius), 1.5f, Random.Range(minCircleDuration, maxCircleDuration)));
        
    }
    IEnumerator SpawnPoint(Vector2 position, float radius, float timeToNextSpawn, float circleDuration)
    {

        nextCircleReady = false;
        foreach (Transform point in transform)
        {
            if(point.gameObject.activeSelf)
            {   continue;}
            
            DrawRing ringDrawer = point.gameObject.GetComponent<DrawRing>();
            
            ringDrawer.SetPosition(position);
            ringDrawer.SetDuration(circleDuration);
            ringDrawer.SetRadius(radius);
            point.gameObject.SetActive(true);

            break;
        }

        yield return new WaitForSeconds(timeToNextSpawn);
        nextCircleReady = true;
    }
}
