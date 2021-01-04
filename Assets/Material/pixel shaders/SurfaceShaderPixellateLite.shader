Shader "SurfaceShaderPixellateLite"
{
		Properties
		{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Albedo (RGB)", 2D) = "white" {}
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("Metallic", Range(0,1)) = 0.0
			_Columns("Pixel Columns", Float) = 64
			_Rows("Pixel Rows", Float) = 64
			_BrightVal("Brightness Value",Float) = 1.3
		}
			SubShader
			{
				Tags { "RenderType" = "Opaque" }
				LOD 200

				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
				#pragma surface surf Standard fullforwardshadows

				// Use shader model 3.0 target, to get nicer looking lighting
				#pragma target 3.0

				sampler2D _MainTex;

				struct Input
				{
					float2 uv_MainTex;

				};

				half _Glossiness;
				half _Metallic;
				fixed4 _Color;
				float _Columns;
				float _Rows;
				float _BrightVal;

				// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
				// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
				// #pragma instancing_options assumeuniformscaling
				UNITY_INSTANCING_BUFFER_START(Props)
					// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				void surf(Input IN, inout SurfaceOutputStandard o)
				{
					// Albedo comes from a texture tinted by color
					float2 uv = IN.uv_MainTex;
					//pixelated effects
					uv.x *= _Columns;
					uv.y *= _Rows;
					uv.x = round(uv.x);
					uv.y = round(uv.y);
					uv.x /= _Columns;
					uv.y /= _Rows;
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex)  ;
					o.Albedo = c.rgb * _BrightVal;

					// Metallic and smoothness come from slider variables
					o.Metallic = _Metallic;
					o.Smoothness = _Glossiness;
					o.Alpha = c.a;
				}
				ENDCG
			}
				FallBack "Diffuse"

}
