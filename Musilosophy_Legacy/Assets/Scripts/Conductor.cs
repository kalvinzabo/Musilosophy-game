using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour      //this idea was taken from https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity
//and then adapted to my game
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    float _secPerBeat, _firstBeatOffset = 0.039f;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats, nextBeatToPlay;

    //How many seconds have passed since the song started
    public float dspSongTime;
    AudioSource musicSource;
    PointSpawner spawner;
    public AudioClip beep;
    public TextAsset beatsToPlay;
    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        spawner = GameObject.Find("Points").GetComponent<PointSpawner>();

        _secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        nextBeatToPlay = 1f;

        StartCoroutine(GetNextBeatFromText());
    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - _firstBeatOffset)  ;

        //determine how many beats since the song started
        songPositionInBeats = songPosition / _secPerBeat;

        if(songPositionInBeats >= nextBeatToPlay - 1)
        {
            spawner.SpawnPoint(nextBeatToPlay * _secPerBeat);
            //empezar a dibujar. Va a estar ligeramente atrasado sin embargo, pero quizá se puede corregir en el otro metodo haciendo que si que de por argumento un momento exacto para terminar.
        }        
    }

    IEnumerator GetNextBeatFromText()   //los beats del texto dicen justo un beat antes de lo que debe sonar: si tiene que sonar en 3 pone 2.
    {
        string text = beatsToPlay.text;

        var lines = text.Split("\n"[0]);        //MENOS MAL QUE HE ENCONTRADO ESTO

        foreach(string line in lines)
        {
            //line = reader.ReadLine();            
            if(line == "")
            {   continue;}

            nextBeatToPlay = float.Parse(line) + 1;
            while(songPositionInBeats < nextBeatToPlay - 1)     //la siguiente iteracion del foreach se hara cuando estemos a menos de un beat de distancia del siguiente beat a tocar, porque sabremos que ya se habrá enviado.
            {   yield return null;}
            //yield return new WaitForSeconds(((nextBeatToPlay - 1) - songPositionInBeats) * _secPerBeat);
        }
        Debug.Log("finished");
        this.enabled = false;
    }
}
