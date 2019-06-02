Shader "Custom/IanVisualizerLocal"
{
	Properties
	{
		_RimColor("Rim Color", Color) = (1, 0, 0, 1)
		_BaseColor("Base Color", Color) = (0, 0, 0, 1)
		_Amplify("Amplify", Float) = 0.0
	}
	SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 vertexInWorldCoords : TEXCOORD1;
				float3 normal : NORMAL;
			};

			float4 _RimColor;
			float4 _BaseColor;
			float _Amplify;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.vertexInWorldCoords = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 n = normalize(i.normal);
				float3 v = normalize(_WorldSpaceCameraPos.xyz - i.vertexInWorldCoords);
				float ndotv = dot(n, v) * _Amplify;

				return lerp(_RimColor, _BaseColor, ndotv);
			}
			ENDCG
		}
	}
}
