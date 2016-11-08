Shader "Custom/WierdOutline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RimModifier("Rim Modifier", Range(0,1)) = 0.5
		_DotProduct("Rim effect", Range(-1,1)) = 0.25
		_AmbientColor ("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue("Ambient Intensity", Range(0,2)) = 1
	}
	SubShader {

			Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
		
	Tags {"Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "true"}
		Cull Back
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types

		#pragma surface surf Lambert alphatest:blend vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		
		sampler2D _MainTex;
		float3 _PlayerPosition;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
		};
		 void vert (inout appdata_full v) {
          v.vertex.xyz += v.normal * 1;
		}
		//float3 _PlayerPos;
		float _DotProduct;
		float _RimModifier;
		float _Radius;
		fixed4 _Color;
		fixed4 _AmbientColor;
		float _MySliderValue;
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color

			fixed4 c = tex2D(_MainTex,IN.uv_MainTex) * pow((_AmbientColor),_MySliderValue);
			
			float border =1- max((abs(dot(IN.viewDir,IN.worldNormal))),_RimModifier);
			float alpha = (border * (1 - _DotProduct) + _DotProduct) ;
			c.a = alpha;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = alpha;
		}
		ENDCG
		


	}
	FallBack "Diffuse"
}
