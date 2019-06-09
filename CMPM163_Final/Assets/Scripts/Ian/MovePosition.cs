// Ian Rapoport

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{
    public Vector3 position1;
    public Vector3 position2;
    public float moveSpeed;

    private bool moveTowards1;
    private float d;
    private float threshold = 0.1f;

    private void Awake()
    {
        d = Vector3.Distance(position1, position2);
    }

    void Update()
    {
        if (moveTowards1)
        {
            if (Vector3.Distance(position2, transform.position) + threshold >= d)
            {
                moveTowards1 = false;
            }
        } 
        else
        {
            if (Vector3.Distance(position1, transform.position) + threshold >= d)
            {
                moveTowards1 = true;
            }
        }

        Vector3 destination = moveTowards1 ? position1 : position2;

        transform.position += Vector3.Normalize(destination - transform.position) * Time.deltaTime * moveSpeed;
    }
}
