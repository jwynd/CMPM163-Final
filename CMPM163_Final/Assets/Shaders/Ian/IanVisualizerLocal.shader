Shader "Custom/IanVisualizerLocal"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (0, 0, 0, 1)
		_Center("Center", Vector) = (0, 0, 0, 0)
		_Length("Length", Float) = 5.0
		_Count("Count", Int) = 1
	}

	SubShader
	{

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles

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
				float4 rawVertex : TEXCOORD0;
				float3 vertexInWorldCoords : TEXCOORD1;
				float3 normal : NORMAL;
			};

			float4 _BaseColor;
			int _Count;
			float4[] _Colors = float4[100];
			float[] _Magnitudes = float[100];
			float4 _Center;
			float _Length;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.rawVertex = v.vertex;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.vertexInWorldCoords = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 offset = i.rawVertex - _Center;
				float bound = _Length / 2.0;
				if (abs(offset.x) < bound)
				{
					//return _Colors[0];
					return float4(1.0, 1.0, 1.0, 1.0);
				}
				else 
				{
					return _BaseColor;
				}
			}
			ENDCG
		}
	}
}
