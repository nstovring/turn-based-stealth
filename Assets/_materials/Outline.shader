Shader "Custom/Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Radius("Radius", Range(0.5,1)) = 0.5
		_RadiusColor("Radius Color", Color) = (1,1,1,1)
		_Cutoff("Cut Off", float) = 0.5
	}
	SubShader {
		
		
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "true"}
		Cull Back
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float3 _PlayerPosition;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		//float3 _PlayerPos;
		float _Radius;
		fixed4 _Color;
		fixed4 _RadiusColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;
			float dist = distance(_PlayerPosition, IN.worldPos) *_Radius;
			float3 playerToPixel = _PlayerPosition - IN.worldPos;
			float dotProd = saturate(dot(playerToPixel,IN.viewDir));
			float gradientFallOff = dist * max(0.2,dotProd);

			c = _Color;
			c.a = gradientFallOff;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG

		Tags {"Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "true"}
		Cull Back
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard alpha:blend noshadow 

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		float3 _PlayerPos;
		float _Radius;
		fixed4 _Color;
		fixed4 _RadiusColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			_PlayerPos.y = 0;
			float3 worldPos = IN.worldPos;
			worldPos.y = 0;
			fixed4 c;
			float dist = distance(_PlayerPos, worldPos) *_Radius;
			float3 playerToPixel = _PlayerPos- worldPos;
			float dotProd = saturate(dot(playerToPixel,IN.viewDir));
			float gradientFallOff = dist * max(0.2,dotProd);

			c = _Color;
			c.a = saturate(gradientFallOff);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
