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
    public bool lerpColors = false;
    public bool lerpMagnitudes = false;
    public bool useWorldPositions = false;
    public float lerpSpeed = 1f;

	private Renderer _renderer;
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

        _renderer = GetComponent<Renderer>();

        _renderer.material.shader = Shader.Find("Custom/IanVisualizerLocal");

        _aveMag = new float[colors.Length];

        _renderer.material.SetColorArray("_Colors", colors);
        _renderer.material.SetInt("_Count", colors.Length);
    }

	void Update ()
    {

        int partitions = colors.Length;
        float[] newMags = new float[partitions];
        float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; 

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / partitions){
                newMags[(int)partitionIndx] += AudioReader.Instance.SpectrumData [i] / (512/partitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		for(int i = 0; i < partitions; i++)
		{
            newMags[i] = newMags[i]*1000;
			if (newMags[i] > 100) {
                newMags[i] = 100;
			}
            newMags[i] = Mathf.Max(lowMag, Mathf.Min(highMag, newMags[i]));
            newMags[i] -= lowMag;
            newMags[i] /= (highMag - lowMag);

            if (lerpMagnitudes)
            {
                _aveMag[i] = Mathf.Lerp(_aveMag[i], newMags[i], lerpSpeed * Time.deltaTime);
            }
            else
            {
                _aveMag[i] = newMags[i];
            }
        }

        SetShaderValues();
    }

    

    void SetShaderValues()
    {
        _renderer.material.SetFloat("_Length", length);
        _renderer.material.SetFloat("_Bottom", bottom);
        _renderer.material.SetFloat("_Top", top);
        _renderer.material.SetVector("_Center", new Vector4(center.x, center.y, center.z, 0f));
        _renderer.material.SetFloatArray("_Magnitudes", _aveMag);
        _renderer.material.SetInt("_LerpColors", lerpColors ? 1 : 0);
        _renderer.material.SetInt("_UseWorldPositions", useWorldPositions ? 1 : 0);
    }
}

