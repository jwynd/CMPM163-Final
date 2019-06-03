using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertAudio : MonoBehaviour {

    public float lowMag;
    public float highMag;
    public Color[] colors;
    public float length;
    public Vector3 center;

    private AudioSource _audioSource;
	private Renderer _renderer;
    private static float[] _spectrumData;
    private float[] aveMag;

    private void Awake()
    {
        if (colors.Length == 0)
        {
            Debug.LogError("No colors assigned");
        }
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();

        _renderer.material.shader = Shader.Find("Custom/IanVisualizerLocal");

        aveMag = new float[colors.Length];
        _spectrumData = new float[512];

        _renderer.material.SetColorArray("_Colors", colors);
    }

	void Update ()
    {
        GetSpectrumAudioSource();

        int partitions = colors.Length;
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; 

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / partitions){
				aveMag[(int)partitionIndx] += _spectrumData [i] / (512/partitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		for(int i = 0; i < partitions; i++)
		{
			aveMag[i] = aveMag[i]*1000;
			if (aveMag[i] > 100) {
				aveMag[i] = 100;
			}
            float newMag = Mathf.Max(lowMag, Mathf.Min(highMag, aveMag[i]));
            newMag -= lowMag;
            newMag /= (highMag - lowMag);
            aveMag[i] = newMag;
        }

        SetShaderValues();
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.Hanning);
    }

    void SetShaderValues()
    {
        _renderer.material.SetFloat("_Length", length);
        _renderer.material.SetVector("_Center", new Vector4(center.x, center.y, center.z, 0f));
        _renderer.material.SetFloatArray("_Magnitudes", aveMag);
    }


}

