// Ian Rapoport

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;
    void Awake(){
        rotateSpeed = Random.Range(-rotateSpeed, rotateSpeed);
        while(rotateSpeed == 0) rotateSpeed = Random.Range(-rotateSpeed, rotateSpeed);
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotateSpeed);
    }
}
