Shader "Custom/UnlitSpaceColor"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Gradient ("Texture", 2D) = "white" {}
		_Darkness ("Darkness", Float) = 0.6
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
			float _Darkness;
    		sampler2D _MainTex;
    		sampler2D _Gradient;

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
				float4 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv = float4(v.texcoord.xy, 0., 0.);

				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				float4 height = tex2D(_MainTex, i.uv.xy);
				float4 col = lerp(float4(0.,0.,0.,1.), tex2D(_Gradient, float2(.5, height.x)), height.x * _Darkness);
				return col;
			}

			ENDCG
		}
	}
}
