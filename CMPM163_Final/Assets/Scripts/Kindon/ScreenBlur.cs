using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenBlur : MonoBehaviour
{
    private Material shader;

    [SerializeField]
    [Range(0, 50)]
    public int steps = 0;
    // Start is called before the first frame update
    void Start()
    {
        shader = new Material(Shader.Find("Custom/ScreenSpaceBlurEffect"));
        Debug.Log(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(shader != null)
        {
            shader.SetFloat("_Steps", steps);
            Graphics.Blit(source, destination, shader);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }
}
