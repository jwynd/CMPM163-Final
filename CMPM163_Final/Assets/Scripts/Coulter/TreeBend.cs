using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBend : MonoBehaviour
{
	Renderer mainRend;
    float displacement;
    ParticleSystem particles;
    ParticleSystemRenderer particleRend;
    ParticleSystem.MainModule partMain;


    void Awake () {
		mainRend = GetComponent<MeshRenderer> ();
        displacement = 0f;
        particles = GetComponentInChildren<ParticleSystem>();
        particleRend = particles.GetComponent<ParticleSystemRenderer>();
        partMain = particles.main;
	}
	

	void Update () {

		int numPartitions = 3;
		float[] aveMag = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; 

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		for(int i = 0; i < numPartitions; i++)
		{
			aveMag[i] = (float)0.5 + aveMag[i]*100;
			if (aveMag[i] > 100) {
				aveMag[i] = 100;
			}
		}

		float mag1 = aveMag[0];
        float mag2 = aveMag[1];
        float mag3 = aveMag[2];

        if (mag1 > 1.0)
        {
            displacement = 1.0f;
        }
        if (mag2 > 1.0)
        {
            particles.Emit(1);
        }
        displacement = Mathf.Lerp(displacement, 0, Time.deltaTime);
        mainRend.material.SetFloat("_Amount1", displacement);
        partMain.startSize = displacement * Random.Range(0.2f, 0.8f);
        particleRend.material.SetFloat("_Amount1", displacement);
    }


}

