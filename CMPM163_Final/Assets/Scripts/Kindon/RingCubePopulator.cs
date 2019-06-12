using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCubePopulator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject vPrefab;
    float timer = 0;

    float bias = .2f;

    GameObject [] cubes = new GameObject[64];

    float [] prevAudioValues = new float[64];

    Vector3 [] restScale = new Vector3[64];

    Color restCol;

    

    Renderer [] rend = new Renderer[64];
    void Start()
    {
        for(int i = 0; i < 64; i++){
            GameObject instance = Instantiate(vPrefab, this.transform.position, transform.rotation, transform);
            transform.eulerAngles = new Vector3(0,-5.625f * i, 0);
            instance.transform.position = Vector3.forward * 30;
            instance.name = "Child " + i;
            cubes[i] = instance;
            prevAudioValues[i] = 0f;
            restScale[i] = cubes[i].transform.localScale;
            //cubeColors[i] = Color.black;
        }
        restCol = GetComponentsInChildren<Renderer>()[0].material.GetColor("_OutlineColor");
        rend = (GetComponentsInChildren<Renderer>());
        
        //mat.shader = Shader.Find("Custom/KindonOutlineShader");
    }

    bool checkBias(int i){
        var prevAudioVal = prevAudioValues[i];
        var audioVal = SpectrumData.spectrumData[i];

        if(prevAudioVal > bias && audioVal <= bias/i){
            return true;
        }

        if(prevAudioVal <= bias && audioVal > bias/i){
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        /* foreach(var ma in rend){
            ma.material.SetColor("_OutlineColor", SpectrumData.col);
        }*/
        //if(timer > .1f){
            for(int i = 0; i < cubes.Length; i++){
                if(cubes != null){
                    if(checkBias(i)){
                        cubes[i].transform.localScale = Vector3.Lerp(cubes[i].transform.localScale, new Vector3(1, SpectrumData.spectrumBuffer[i] * 50 * (i+1), 1), 3f * Time.deltaTime);
                        prevAudioValues[i] = SpectrumData.spectrumBuffer[i];
                        rend[i].material.SetColor("_OutlineColor", Color.Lerp(SpectrumData.cubeOldColors[i], SpectrumData.cubeNewColors[i], Time.deltaTime*400f));
                    }else{
                        rend[i].material.SetColor("_OutlineColor", Color.Lerp(SpectrumData.cubeOldColors[i],  restCol, Time.deltaTime*800f));
                    }
                    
                }
            }
 
        //}else{
            for(int i = 0; i < cubes.Length; i++){
                cubes[i].transform.localScale = Vector3.Lerp(cubes[i].transform.localScale, restScale[i], 2f * Time.deltaTime);
            }
           //transform.localScale = Vector3.Lerp(transform.localScale, restScale, 2f * Time.deltaTime);
        //}
       
        timer += Time.deltaTime;
    }
}
