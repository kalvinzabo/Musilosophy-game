using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public float minRadius, maxRadius;
    float _SCREENWIDTH, _WIDTHOFFSET, _SCREENHEIGHT, _HEIGHTOFFSET;
    Vector2 _SCREENCORNER, _randomPos;  //The corner is the origin, bottom left.

    // bool nextCircleReady = true;
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
    public void SpawnPoint(float circleDuration)
    {
        _randomPos = _SCREENCORNER + new Vector2(Random.Range(
            _WIDTHOFFSET, _SCREENWIDTH - _WIDTHOFFSET),     //x position of the new circle
            Random.Range(
            _HEIGHTOFFSET, _SCREENHEIGHT - _HEIGHTOFFSET));     //y position of the new circle

        // nextCircleReady = false;
        foreach (Transform point in transform)
        {
            if(point.gameObject.activeSelf)
            {   continue;}
            
            DrawRing ringDrawer = point.gameObject.GetComponent<DrawRing>();
            
            ringDrawer.SetPosition(_randomPos);
            ringDrawer.SetRadius(Random.Range(minRadius, maxRadius));
            ringDrawer.SetDuration(circleDuration);

            point.gameObject.SetActive(true);

            break;
        }

        // yield return new WaitForSeconds(timeToClose);
        // nextCircleReady = true;
    }
}


//el conductor llama al spawner y le dice "quiero un circulo que se cierre en este momento preciso". Para eso solo tiene que darle el momento preciso.
//De ahi el spawner puede calcular el tiempo que queda y asignarlo a circleDuration al llamar la corrutina. La cosa es cuando decide el conductor llamar a esta orden.
//Voy a fijar la duracion en 1 beat. Asi todo sera mas sencillo.

//Añadir un contador de puntos que suba con cada circulo que se cierra, para hacer
//creer al jugador que lo estaba consiguiendo.
//decirle que tiene que conseguir pulsarlo "lo mas cerca posible del momento en el que cierra" y dar diferentes cantidades de puntos de forma aleatoria para que
//piense que está contando cada click cuanto se acerca. Poner una pantalla de "puntuaciones" al principio para engañar, y hacer enfasis en que es muy importante jugar
//con un raton.
