using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumData : MonoBehaviour
{
    private float[] spectrumData;
    public static float spectrumValue {get; private set;}


    // Start is called before the first frame update
    void Start()
    {
        spectrumData = new float[128];
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrumData,0,FFTWindow.Hamming);
        if(spectrumData != null && spectrumData.Length > 0){
            spectrumValue = spectrumData[1] * 100;
            //Debug.Log("SpecVal is not null or 0");
        }
    }
}
