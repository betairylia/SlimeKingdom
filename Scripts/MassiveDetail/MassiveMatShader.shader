Shader "Custom/MassiveMatShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_LightColor ("LightColor", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Top ("Mesh y Max", float) = 0
		_BillBoard ("isBillboard (negative number)", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf MassiveGrass fullforwardshadows addshadow vertex:vert
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:setup

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _Top;
		float _BillBoard;

		struct Input {
			float2 coord;
		};

		struct propDataInstance
		{
			float4x4 modelWorld;
			float3 dirc;
			//float3 tang;
			bool visible;
		};

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		StructuredBuffer<propDataInstance> propBuffer;
#endif

		void setup() {}

		// float4x4 inverse(float4x4 input)
		// {
		// 	#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
		// 	//determinant(float3x3(input._22_23_23, input._32_33_34, input._42_43_44))
		//
		// 	float4x4 cofactors = float4x4(
		// 		 minor(_22_23_24, _32_33_34, _42_43_44),
		// 		-minor(_21_23_24, _31_33_34, _41_43_44),
		// 		 minor(_21_22_24, _31_32_34, _41_42_44),
		// 		-minor(_21_22_23, _31_32_33, _41_42_43),
		//
		// 		-minor(_12_13_14, _32_33_34, _42_43_44),
		// 		 minor(_11_13_14, _31_33_34, _41_43_44),
		// 		-minor(_11_12_14, _31_32_34, _41_42_44),
		// 		 minor(_11_12_13, _31_32_33, _41_42_43),
		//
		// 		 minor(_12_13_14, _22_23_24, _42_43_44),
		// 		-minor(_11_13_14, _21_23_24, _41_43_44),
		// 		 minor(_11_12_14, _21_22_24, _41_42_44),
		// 		-minor(_11_12_13, _21_22_23, _41_42_43),
		//
		// 		-minor(_12_13_14, _22_23_24, _32_33_34),
		// 		 minor(_11_13_14, _21_23_24, _31_33_34),
		// 		-minor(_11_12_14, _21_22_24, _31_32_34),
		// 		 minor(_11_12_13, _21_22_23, _31_32_33)
		// 	);
		// 	#undef minor
		// 	return transpose(cofactors) / determinant(input);
		// }

		void vert(inout appdata_full v, out Input o)
		{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			/*float4x4 dircSpace = {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1};
			dircSpace._11_12_13 = propBuffer[unity_InstanceID].tang;
			dircSpace._21_22_23 = propBuffer[unity_InstanceID].dirc;
			dircSpace._31_32_33 = cross(dircSpace._11_12_13, propBuffer[unity_InstanceID].dirc);*/

			if(propBuffer[unity_InstanceID].visible == false)
			{
				return;
			}

			// if(_BillBoard > 0.0)
			// {
				float3 deltaPos = propBuffer[unity_InstanceID].modelWorld._14_24_34;
				float3 mPos = mul(propBuffer[unity_InstanceID].modelWorld, v.vertex).xyz - deltaPos;

				float3 tang = normalize(float3(propBuffer[unity_InstanceID].dirc.y, -propBuffer[unity_InstanceID].dirc.x, 0));
				/*float4x4 modelModel = float4x4(tang.xyz, 0, propBuffer[unity_InstanceID].dirc.xyz, 0, cross(tang, propBuffer[unity_InstanceID].dirc).xyz, 0, 0, 0, 0, 1);*/
				float4x4 modelModel = float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
				modelModel._12_22_32 = propBuffer[unity_InstanceID].dirc.xyz;

				float3 nPos = mul(modelModel, float4(mPos, 1.0f)).xyz;

				v.vertex.xyz = deltaPos + mPos * (_Top - abs(mPos.y) / _Top) + nPos * abs(mPos.y) / _Top;
				//v.vertex = mul(UNITY_MATRIX_VP, v.vertex);
			// }
			// else
			// {
				// float3 deltaPos = propBuffer[unity_InstanceID].modelWorld._14_24_34;
				// v.vertex = mul(UNITY_MATRIX_MV, float4(deltaPos, 1.0)) + float4(v.vertex.x, v.vertex.y, 0.0, 0.0);
				// /*v.vertex = mul(UNITY_MATRIX_T_MV, v.vertex);
				//
				// v.vertex = mul(propBuffer[unity_InstanceID].modelWorld, v.vertex);
				// v.vertex = mul(UNITY_MATRIX_V, v.vertex);*/
				// v.vertex = mul(inverse(UNITY_MATRIX_MV), v.vertex);
			// }
#endif
			o.coord = v.texcoord;
		}

		fixed4 _Color, _LightColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		/*UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END*/

		float4 LightingMassiveGrass(SurfaceOutput s, float3 lightDir, half3 viewDir, half atten)
		{
			float4 c;
			float l = pow(dot(viewDir, half3(1, 0, 0)), 8);
			l = clamp(l, 0, 1);
			c.rgb = (s.Albedo * 0.5f + 0.4f * l * _LightColor) * atten;
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			// Albedo comes from a texture tinted by color
			if(propBuffer[unity_InstanceID].visible == false)
			{
				clip(-1.0);
			}

			fixed4 c = tex2D(_MainTex, IN.coord);

			if(c.a <= 0.33f)
			{
				clip(-1.0);
			}

			c = c * _Color;

			if(_BillBoard > 0)
			{
				c.rgb = 0.4 * c.rgb * min(1.0f, 1.0f * IN.coord.y) + 0.6 * c.rgb;
			}

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			/*o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;*/
			o.Alpha = c.a;
#endif
		}
		ENDCG
	}
	FallBack "Diffuse"
}
