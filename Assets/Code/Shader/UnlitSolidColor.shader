Shader "Custom/Unlit Solid Color"
{
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1) 
	}

	SubShader
	{
		Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Opaque" }
		Lighting Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		Cull Off

		Pass 
		{
			AlphaTest Greater .25

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
//				float4 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = _Color; // v.color;
//				o.uv = float4(v.texcoord.xy, 0., 0.);

				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				return i.color;
			}

			ENDCG
		}
	}
}
