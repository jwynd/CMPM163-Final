﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class EmitAtHeight : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    /*void Update()
    {
        Debug.Log(this.gameObject.name + " Should be emitting "+(int)Mathf.Floor(this.transform.position.z * 5) +" particles");
        ps.Emit((int)Mathf.Floor(this.transform.position.z * 5));
    }*/
    public void emitAtRotation(float rot){
        ps.Emit((int)Mathf.Floor(rot*6));
    }
}
