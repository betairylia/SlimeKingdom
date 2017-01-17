Shader "Custom/Rim" {
	//属性域  
	Properties
	{
		//主纹理属性  
		_MainTex("Texture", 2D) = "white" {}
		//漫反射颜色
		_AlbedoColor("Albedo Color", Color) = (1,1,1,1)
		//自发光颜色
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		//边缘光颜色值  
		_RimColor("Rim Color", Color) = (1,1,1,1)
		//边缘光强度值  
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader
	{
		//标明渲染类型是不透明的物体  
		Tags{ "RenderType" = "Opaque" }
		//标明CG程序的开始  
		CGPROGRAM
		//声明表面着色器函数  
		#pragma surface surf Lambert  
		//定义着色器函数输入的参数Input  
		struct Input 
		{
			//主纹理坐标值  
			float2 uv_MainTex;
			//法线贴图坐标值  
			float2 uv_BumpMap;
			//视图方向  
			float3 viewDir;
		};
		//声明对属性的引用  
		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _RimColor;
		float _RimPower;
		float4 _AlbedoColor, _EmissionColor;
		//表面着色器函数  
		void surf(Input IN, inout SurfaceOutput o) 
		{
			//赋值颜色信息  
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _AlbedoColor;
			o.Gloss = 0.8;
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			//赋值自发光颜色信息  
			o.Emission = _EmissionColor + _RimColor.rgb * pow(rim, _RimPower);
		}
		//标明CG程序的结束  
		ENDCG
	}
	Fallback "Diffuse"
}
