using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketMover : MonoBehaviour
{
    public float[] buckets = new float[8];
    [SerializeField] private int[] cutoffs = new int[8];

    void OnValidate(){
        if(cutoffs.Length != 8){
            Debug.LogError("There must be 8 cutoffs");
        }
    }

    void start(){
        for(int i = 0; i < 8; i++){
            buckets[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        for(int j = 0; j < buckets.Length; ++j){
            for(int k = j==0?0:cutoffs[j-1]; k < cutoffs[j]; ++k){
                //Debug.Log(i);
                buckets[j] += AudioPeer.spectrumData[i++];
            }
        }

        for(int j = 0; j < buckets.Length; ++j){
            //Debug.Log(j);
            this.transform.GetChild(j).localPosition = new Vector3(this.transform.GetChild(j).localPosition.x, buckets[j], 0);
        }
    }
}
