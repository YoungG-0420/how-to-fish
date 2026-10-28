Shader "Shader Graphs/Grass" {
	Properties {
		_Color ("Color", Vector) = (0.3803922,0.454902,0.282353,1)
		_TopColor ("TopColor", Vector) = (0.5882353,0.6235294,0.2980392,1)
		_Smoothness ("Smoothness", Float) = 0
		[NoScaleOffset] _Texture ("Texture", 2D) = "white" {}
		_WindColorMulti ("WindColorMulti", Float) = 0.5
		_WindSpeed ("WindSpeed", Float) = 3.2
		_WindSpeed2 ("WindSpeed2", Float) = 1
		_WindStrength ("WindStrength", Float) = 0.15
		_WindDensity ("WindDensity", Float) = 0.2
		[NoScaleOffset] _Noise_Map ("Noise Map", 2D) = "white" {}
		_Noise_Map_Scale ("Noise Map Scale", Vector) = (0.04,0.02,0,0)
		_Noise_Remap ("Noise Remap", Vector) = (0.9,1,0,0)
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	SubShader{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			TEXTURE2D(_Texture); SAMPLER(sampler_Texture);
			half4 _Color; half4 _TopColor; float _WindSpeed; float _WindStrength; float _WindDensity;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				input.pos.x += sin(_Time.y * _WindSpeed + input.pos.x * _WindDensity) * _WindStrength * saturate(input.uv.y);
				output.pos = TransformObjectToHClip(input.pos.xyz);
				output.uv = input.uv;
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				half4 tex = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, input.uv);
				return tex * lerp(_Color, _TopColor, saturate(input.uv.y));
			}

			ENDHLSL
		}
	}
	Fallback "Universal Render Pipeline/Unlit"
}
