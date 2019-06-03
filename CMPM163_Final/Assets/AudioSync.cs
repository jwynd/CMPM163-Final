﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSync : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;
    private float prevAudioVal;
    private float audioVal;
    private float timer;
    protected bool isBeat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    public virtual void OnBeat(){
        Debug.Log("beat");
        timer = 0;
        isBeat = true;
    }
    public virtual void OnUpdate(){
        prevAudioVal = audioVal;
        audioVal = SpectrumData.spectrumValue;

        if(prevAudioVal > bias && audioVal <= bias){
            if(timer > timeStep){
                OnBeat();
            }
        }

        if(prevAudioVal <= bias && audioVal > bias){
            if(timer > timeStep){
                OnBeat();
            }
        }

        timer += Time.deltaTime;
    }
}
