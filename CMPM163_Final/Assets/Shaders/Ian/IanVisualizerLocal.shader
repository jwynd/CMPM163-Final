Shader "Custom/IanVisualizerLocal"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (0, 0, 0, 1)
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

			float4 _BaseColor;

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
				return _BaseColor;
			}
			ENDCG
		}
	}
}
