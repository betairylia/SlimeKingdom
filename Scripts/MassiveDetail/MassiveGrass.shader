Shader "Unlit/MassiveGrass"
{
	Properties
	{
		albedoMap("Texture", 2D) = "white" {}

		playerPos("Player position", Vector) = (0, 0, 0, 0)
		fadeInRangeNear("Fade In Range (near)", Float) = 10.0
		fadeRange("Fade Range (far - near)", Float) = 5.0
		fadeTexture("Fade Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="CutOut" }
		LOD 100

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
				fixed3 vcolor : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				fixed4 vertexColor : COLOR0;
				fixed4 finalColor : COLOR1;
			};

			fixed4 color;
			float4 playerPos;
			float fadeInRangeNear, fadeRange;

			sampler2D fadeTexture, albedoMap;

			v2f vert(appdata v)
			{
				v2f o;
				float4 vPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);

				float dist = distance(vPos, playerPos);

				o.finalColor = fixed4(1, 1, 1, 1);

				if (dist > fadeInRangeNear)
				{
					o.finalColor.a *= 1.0 - (dist - fadeInRangeNear) / fadeRange;
				}

				o.vertexColor = fixed4(v.vcolor, 1.0);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 albedoColor = tex2D(albedoMap, i.uv);

				if (i.finalColor.a <= 0 || albedoColor.a <= 0.33)
				{
					discard;
				}

				fixed fadeAlpha = tex2D(fadeTexture, i.uv);
				if (i.finalColor.a < fadeAlpha)
				{
					discard;
				}

				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);

				return albedoColor * i.vertexColor;
				//return i.vertexColor;
			}
			ENDCG
		}
	}
}
