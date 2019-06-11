// Ian Rapoport

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertAudio : MonoBehaviour
{
    public Color[] colors;
    public Magnitudes[] magnitudes;
    public float length;
    public Vector3 center;
    public float top;
    public float bottom;
    public bool lerpColors = false;
    public bool lerpMagnitudes = false;
    public bool useWorldPositions = false;
    public float lerpSpeed = 1f;
    public Renderer[] renderers;

    private List<Material> _materials;
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

        if (colors.Length != magnitudes.Length)
        {
            Debug.LogError("Color and magnitudes do not match up");
        }

        _aveMag = new float[colors.Length];
        _materials = new List<Material>();

        foreach (Renderer renderer in renderers)
        {
            _materials.Add(renderer.material);
        }

        foreach (Material material in _materials)
        {
            material.shader = Shader.Find("Custom/IanVisualizer");
            material.SetColorArray("_Colors", colors);
            material.SetInt("_Count", colors.Length);
        }
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
            newMags[i] = Mathf.Max(magnitudes[i].low, Mathf.Min(magnitudes[i].high, newMags[i]));
            newMags[i] -= magnitudes[i].low;
            newMags[i] /= (magnitudes[i].high - magnitudes[i].low);

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
        foreach (Material material in _materials)
        {
            material.SetFloat("_Length", length);
            material.SetFloat("_Bottom", bottom);
            material.SetFloat("_Top", top);
            material.SetVector("_Center", new Vector4(center.x, center.y, center.z, 0f));
            material.SetFloatArray("_Magnitudes", _aveMag);
            material.SetInt("_LerpColors", lerpColors ? 1 : 0);
            material.SetInt("_UseWorldPositions", useWorldPositions ? 1 : 0);
        }
    }

    [System.Serializable]
    public class Magnitudes
    {
        public float low;
        public float high;
    }
}

