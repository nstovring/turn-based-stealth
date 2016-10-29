Shader "Custom/GridEdge" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Center("Center", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0.5
		_RadiusColor("Radius Color", Color) = (1,0,0,1)
		_RadiusWidth("Radius Width", Float) = 2
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector" = "true" }
		Cull Back
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float3 _Center;
		float _Radius;
		fixed4 _RadiusColor;
		float _RadiusWidth;
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color

			float d = distance(_Center, IN.worldPos);
			float squareDiagonalLength = sqrt(pow(_Radius,2) +pow(_Radius,2) );
			fixed4 c;
			if (d > squareDiagonalLength){
			c = tex2D (_MainTex, IN.uv_MainTex) * _RadiusColor;
			}
			else{
			c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			}

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
