using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class EmitAtHeight : MonoBehaviour
{
    [SerializeField][Range(0.01f,1.0f)]private float sensitivity = 0.5f;
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.gameObject.name + " Should be emitting "+(int)Mathf.Floor(this.transform.localPosition.y * 5) +" particles");
        ps.Emit((int)Mathf.Floor(this.transform.localPosition.y * 5));
    }
}
