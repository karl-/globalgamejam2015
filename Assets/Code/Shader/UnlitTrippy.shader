Shader "Custom/Unlit Trippy Color"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Gradient ("Texture", 2D) = "white" {}
		_Mix ("Mix", Float) = 0
		_Speed ("Speed", Float) = 1
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

    		sampler2D _MainTex;
    		sampler2D _Gradient;
    		float4 _MainTex_ST;
    		float _Mix;
    		float _Speed;

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
				o.uv = float4(TRANSFORM_TEX(v.texcoord, _MainTex), 0., 0.);

				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				float4 tex = tex2D(_MainTex, i.uv.xy);
				float val = (tex.x + tex.y + tex.z) / 3. + sin(_Time.y * _Speed);

				return lerp(tex, tex2D(_Gradient, float2(.5, val)), _Mix);
			}

			ENDCG
		}
	}
}
