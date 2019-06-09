// Ian Rapoport

Shader "Custom/IanVisualizer"
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
			uint _Count;
			int _LerpColors;
			int _UseWorldPositions;

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
				float4 vertex;
				if (_UseWorldPositions == 1) 
				{	
					vertex = float4(i.vertexInWorldCoords, 0.0);
				}
				else 
				{
					vertex = i.rawVertex;
				}

				float4 offset = vertex - _Center;
				float bound = _Length / 2.0;

				bool inBound = abs(offset.x) <= bound;
				bool aboveBottom = vertex.y >= _Bottom;

				float segment = _Length / _Count;
				float distanceFromRight = vertex.x - (_Center.x - bound);
				uint index = (int)(distanceFromRight / segment);
				bool withinMagnitude = vertex.y <= _Bottom + _Magnitudes[index] * (_Top - _Bottom);

				if (inBound && aboveBottom && withinMagnitude)
				{
					if (_LerpColors == 1) {
						return lerp(_Colors[0], _Colors[_Count - 1], (float)index / (_Count - 1));
					}
					else {
						return _Colors[index];
					}
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
