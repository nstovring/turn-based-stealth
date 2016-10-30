Shader "Custom/Hologram" {
	Properties {
		_MainTex("My Tex", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_AmbientColor ("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue("Ambient Intensity", Range(0,2)) = 1
		_RimModifier("Rim Modifier", Range(0,1)) = 0.5
		_DotProduct("Rim effect", Range(-1,1)) = 0.25
		_HeightSlider("Height falloff", Range(0,2)) = 1
		_HeightOffset("Height Offset", Range(-2,2)) = 0
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "true" }
		Cull Back 
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Lambert alpha:fade nolighting 
		#pragma surface surf Lambert alpha:fade nolighting
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
			float3 worldPos;
		};

		float _DotProduct;
		float _RimModifier;
		float _HeightSlider;
		float _HeightOffset;
		float4 _AmbientColor;
		half _MySliderValue;
		fixed4 _Color;
		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			float heightScale = (IN.worldPos.y -_HeightOffset) / _HeightSlider;
			
			fixed4 c = tex2D(_MainTex,IN.uv_MainTex) * pow((_Color+_AmbientColor),_MySliderValue);
			o.Albedo = c.rgb;

			float border = max((abs(dot(IN.viewDir,IN.worldNormal))),_RimModifier)/ (heightScale);
			float alpha = (border * (1 - _DotProduct) + _DotProduct) ;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a * alpha;
		}
		ENDCG
		
	}
	FallBack "Diffuse"
}
