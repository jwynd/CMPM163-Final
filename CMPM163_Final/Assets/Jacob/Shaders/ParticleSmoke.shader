Shader "Custom/ParticleSmoke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]_Start ("Start Color", Color) = (1,1,1,1)
        [HDR]_End ("End Color", Color) = (1,1,1,1)
        //Define properties for Start and End Color
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100
        
        Blend One One
        ZWrite off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            uniform sampler2D _MainTex;
            uniform float4 _Start;
            uniform float4 _End;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 uv : TEXCOORD0;
                //float4 uv : TEXCOORD0;
                //Define UV data
            };

            struct v2f
            {   
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 uv : TEXCOORD0;
                //float4 uv : TEXCOORD0;
                //Define UV data
            };

          

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;//UnityObjectToWorldNormal(v.normal); 
                o.uv = v.uv; //Correct this for particle shader
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv.xy;
                float age = i.uv.z;
                //float noise = i.uv.w;
                //Get particle age percentage
                float4 texColor = tex2D(_MainTex, uv);
                //Sample color from particle texture
                float4 sColor = texColor*_Start;
                //Find "start color"
                float4 eColor = _End*texColor.a;
                //Find "end color"

                float4 final = lerp(sColor, eColor, age);
                //Do a linear interpolation of start color and end color based on particle age percentage
               
                return final;
            }
            ENDCG
        }
    }
}
