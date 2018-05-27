// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Swanit/Shines"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float _Tint;

			struct vertexInput{
				fixed4 vertex : POSITION;
				fixed4 coord : TEXCOORD0;
			};

			struct vertexOutput{
				fixed4 pos : SV_POSITION;
				fixed4 uv : TEXCOORD1;
			};

			vertexOutput vert(vertexInput vin){
				vertexOutput vout;
				vout.uv = vin.coord;
				vout.pos = UnityObjectToClipPos(vin.vertex);
				return vout;
			}

			fixed4 frag(vertexOutput vout) : COLOR{
				float f = vout.uv.x + vout.uv.y;// + abs(sin(_Time.y * _Tint));
				fixed4 c = fixed4(f,f,f,1);
				c.rgb += abs(sin(_Time.y)) * _Tint;
				return tex2D(_MainTex, vout.uv) * c;
			}

			ENDCG
		}
	}
}
