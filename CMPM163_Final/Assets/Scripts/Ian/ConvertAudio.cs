using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertAudio : MonoBehaviour {

    public float lowMag;
    public float highMag;
    public Color[] colors;
    public float length;
    public Vector3 center;
    public float top;
    public float bottom;

    private AudioSource _audioSource;
	private Renderer _renderer;
    private static float[] _spectrumData;
    private float[] _aveMag;

    private void Awake()
    {
        if (colors.Length == 0)
        {
            Debug.LogError("No colors assigned");
        }

        if (colors.Length > 128)
        {
            Debug.LogError("Too many colors assigned. 128 max");
        }

        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();

        _renderer.material.shader = Shader.Find("Custom/IanVisualizerLocal");

        _spectrumData = new float[512];

        _renderer.material.SetColorArray("_Colors", colors);
        _renderer.material.SetInt("_Count", colors.Length);
    }

	void Update ()
    {
        GetSpectrumAudioSource();

        int partitions = colors.Length;
        _aveMag = new float[partitions];
        float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; 

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / partitions){
                _aveMag[(int)partitionIndx] += _spectrumData [i] / (512/partitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		for(int i = 0; i < partitions; i++)
		{
			_aveMag[i] = _aveMag[i]*1000;
			if (_aveMag[i] > 100) {
				_aveMag[i] = 100;
			}
            float newMag = Mathf.Max(lowMag, Mathf.Min(highMag, _aveMag[i]));
            newMag -= lowMag;
            newMag /= (highMag - lowMag);
            _aveMag[i] = newMag;
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
        _renderer.material.SetFloat("_Bottom", bottom);
        _renderer.material.SetFloat("_Top", top);
        _renderer.material.SetVector("_Center", new Vector4(center.x, center.y, center.z, 0f));
        _renderer.material.SetFloatArray("_Magnitudes", _aveMag);
    }
}

