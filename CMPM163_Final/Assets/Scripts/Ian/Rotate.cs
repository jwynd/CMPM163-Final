// Ian Rapoport

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * rotateSpeed);
    }
}
