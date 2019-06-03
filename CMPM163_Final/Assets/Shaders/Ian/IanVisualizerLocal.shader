Shader "Custom/IanVisualizerLocal"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (0, 0, 0, 1)
		_Center("Center", Vector) = (0, 0, 0, 0)
		_Bottom("Bottom", Float) = 0.0
		_Top("Top", Float) = 1.0
		_Length("Length", Float) = 5.0
		_Count("Count", Int) = 1
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
				float4 rawVertex : TEXCOORD0;
				float3 vertexInWorldCoords : TEXCOORD1;
				float3 normal : NORMAL;
			};

			float4 _BaseColor;
			float4 _Colors[128];
			float _Magnitudes[128];
			float4 _Center;
			float _Bottom;
			float _Top;
			float _Length;
			int _Count;

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

				bool inBound = abs(offset.x) <= bound;
				bool aboveBottom = i.rawVertex.y >= _Bottom;

				float segment = _Length / _Count;
				float distanceFromRight = i.rawVertex.x - (_Center.x - bound);
				int index = (int)(distanceFromRight / segment);
				bool withinMagnitude = i.rawVertex.y <= _Bottom + _Magnitudes[index] * (_Top - _Bottom);

				if (inBound && aboveBottom && withinMagnitude)
				{
					return _Colors[index];
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
