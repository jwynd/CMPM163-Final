Shader "Custom/KindonOutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainColor("_MainColor", Color) = (0,0,1,0)
		_OutlineColor("_OutlineColor", Color) = (0, 0, 0, 0)
		_OutlineThickness("_OutlineThickness", Float) = 0.2
		_RadialBlendTex("BlendTex", 2D) = "white" {}
		_Emissiveness("Emissiveness", Range(0,10)) = 0
		_Steps("Steps", Float) = 0
        _Cube ("Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Pass
        {
			Name "Outline"
			//Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
				float3 vertexInWorldCoords : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _OutlineThickness;
			float4 _OutlineColor;
			float4 _MainColor;
			float4 _MainTex_TexelSize;
			float _Steps;
			uniform float _Emissiveness;
			uniform float4 _LightColor0;

            v2f vert (appdata v)
            {
                v2f o;
				v.vertex.xyz += float4(v.normal, 1.0) * _OutlineThickness;
				v.uv += float4(v.normal, 1.0) * _OutlineThickness;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 P = i.vertexInWorldCoords.xyz;
				float3 N = normalize(i.normal);
				float3 V = normalize(_WorldSpaceCameraPos - P);
				float3 L = normalize(_WorldSpaceLightPos0.xyz - P);
				float3 H = normalize(L + V);

				float3 Kd = _MainColor.rgb; //Color of object
				float3 Ka = UNITY_LIGHTMODEL_AMBIENT.rgb; //Ambient light
				float3 Ks = _OutlineColor.rgb; //Color of specular highlighting
				float3 Kl = _LightColor0.rgb; //Color of light


				//AMBIENT LIGHT
				float3 ambient = Ka;


				//DIFFUSE LIGHT
				float diffuseVal = max(dot(N, L), 0);
				float3 diffuse = Kd * Kl * diffuseVal;


				//SPECULAR LIGHT
				float specularVal = pow(max(dot(N,H), 0), 10);

				if (diffuseVal <= 0) {
					specularVal = 0;
				}

				float3 specular = Ks * Kl * specularVal;

				float4 texColor = tex2D(_MainTex, i.uv * _Time/50);
				//FINAL COLOR OF FRAGMENT

				return float4(_OutlineColor * _Emissiveness + ambient + diffuse + specular, 1.0)*texColor;
            }

            ENDCG
        }

		Pass
		{
			Name "Body"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 vertex : SV_POSITION;
                float3 vertexInWorldCoords : TEXCOORD1;
                float3 normalInWorldCoords : NORMAL1;
			};

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.normalInWorldCoords = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			float4 _MainColor;
               float4 _OutlineColor;
               uniform float4 _LightColor0;
               sampler2D _RadialBlendTex;
               samplerCUBE _Cube;
			fixed4 frag (v2f i) : SV_Target
            {
                float3 P = i.vertexInWorldCoords.xyz;

                //get normalized incident ray (from camera to vertex)
                float3 vIncident = normalize(P - _WorldSpaceCameraPos);

                //reflect that ray around the normal using built-in HLSL command
                float3 vReflect = reflect( vIncident, i.normalInWorldCoords );


                //use the reflect ray to sample the skybox
                float4 reflectColor = texCUBE( _Cube, vReflect );

                //refract the incident ray through the surface using built-in HLSL command
                float3 vRefract = refract( vIncident, i.normalInWorldCoords, 0.5 );

                //float4 refractColor = texCUBE( _Cube, vRefract );


                float3 vRefractRed = refract( vIncident, i.normalInWorldCoords, 0.1 );
                float3 vRefractGreen = refract( vIncident, i.normalInWorldCoords, 0.4 );
                float3 vRefractBlue = refract( vIncident, i.normalInWorldCoords, 0.7 );

                float4 refractColorRed = texCUBE( _Cube, float3( vRefractRed ) );
                float4 refractColorGreen = texCUBE( _Cube, float3( vRefractGreen ) );
                float4 refractColorBlue = texCUBE( _Cube, float3( vRefractBlue ) );
                float4 refractColor = float4(refractColorRed.r, refractColorGreen.g, refractColorBlue.b, 1.0);
                 return float4(lerp(reflectColor, refractColor, 0.5).rgb, 1.0);

        }
			ENDCG
		}
    }
}
