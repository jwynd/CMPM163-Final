using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumData : MonoBehaviour
{
    public static float[] spectrumData {get; private set;}
    public static float spectrumValue {get; private set;}

    public static float[] spectrumBuffer {get; private set;}

    public static Color [] cubeOldColors = new Color[64];
    public static Color [] cubeNewColors = new Color[64];
    float[] bufferDecrease;

    public static Color col {get; private set;}


    // Start is called before the first frame update
    void Start()
    {
        spectrumData = new float[128];
        spectrumBuffer = new float[128];
        bufferDecrease = new float[128];

        //mat = new Material(Shader.Find("Custom/KindonOutlineShader"));
        col = Color.red;
    }

    void CalculateColor(){
        for(int i = 0; i < 64; i++){
            cubeOldColors[i] = cubeNewColors[i];
            cubeNewColors[i] = new Color(spectrumBuffer[i] * 10f * (i+1), spectrumBuffer[i] * 60f * (i+1), spectrumBuffer[i] * 80f * (i+1), 1f);
        }
    }

    void BandBuffer(){
        for(int i = 0; i < 128; i++){
            if(spectrumData[i] > spectrumBuffer[i]){
                spectrumBuffer[i] = spectrumData[i];
                bufferDecrease[i] = .005f;
            }
            if(spectrumData[i] < spectrumBuffer[i]){
                spectrumBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrumData,0,FFTWindow.Hamming);
        if(spectrumData != null && spectrumData.Length > 0){
            spectrumValue = spectrumData[1] * 100;
            //Debug.Log("SpecVal is not null or 0");
        }
        CalculateColor();
        BandBuffer();
    }
}
