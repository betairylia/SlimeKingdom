Shader "Unlit/Forest_LightShafts"
{
	Properties
	{
		ShaftNoiseTex ("Light Shaft Noise Texture", 2D) = "white" {}
		ShaftMainColor("Light Shaft Main Color", Color) = (1,1,1,1)
		UpBottomRatio("Up-Bottom Ratio", float) = 2.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		/*BlendOp RevSub*/
		ZWrite Off
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D ShaftNoiseTex;
			float4 ShaftMainColor;
			float UpBottomRatio;

			v2f vert (appdata v)
			{
				//todo: billboarding

				v2f o;
				o.vertex = v.vertex;

				//This will cause triangles being streched
				// => Light shafts being streched.
				/*if(o.vertex.y < 0.0)
					o.vertex.x *= 2.0;
				else
					o.vertex.x *= 0.3;*/
				o.vertex = mul(UNITY_MATRIX_MVP, o.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				i.uv.x = (i.uv.x - 0.5) / ((1.0 / UpBottomRatio) * (1.0 + (UpBottomRatio - 1.0) * (1.0-i.uv.y))) + 0.5;

				/*float4 finalColor = float4(0, 0, 0, 1);
				if(abs(i.uv.x - 0) < 0.01) finalColor.r = 1;
				if(abs(i.uv.x - 0.25) < 0.01) finalColor.r = 1;
				if(abs(i.uv.x - 0.5) < 0.01) finalColor.r = 1;
				if(abs(i.uv.x - 0.75) < 0.01) finalColor.r = 1;
				if(abs(i.uv.x - 1.0) < 0.01) finalColor.r = 1;*/

				if(i.uv.x < 0 || i.uv.x > 1.0)
					discard;

				// sample the texture
				float4 strength = tex2D(ShaftNoiseTex, float2(i.uv.x, (_Time.x)));
				float4 finalColor = ShaftMainColor;
				finalColor.a = max(0.0, (1.0 - (1.0 - i.uv.y) * (1.0 + 0.5 * strength.r)));
				finalColor.a *= 1.0 - pow(5.0 * (max(0.0, i.uv.x - 0.8) + max(0.0, 0.2 - i.uv.x)), 2.0);
				finalColor.a *= 1.0 - (max(0.0, i.uv.y - 0.85) / 0.15);

				finalColor.a *= 0.6;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return finalColor;
			}
			ENDCG
		}
	}
}
