using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReader : MonoBehaviour
{
    private AudioSource _audioSource;

    public float[] SpectrumData { get; private set; }

    public static AudioReader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
        SpectrumData = new float[512];
    }

    void Update()
    {
        _audioSource.GetSpectrumData(SpectrumData, 0, FFTWindow.Hanning);
    }
}
