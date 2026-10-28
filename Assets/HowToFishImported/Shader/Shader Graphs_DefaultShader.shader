Shader "Shader Graphs/DefaultShader" {
	Properties {
		[NoScaleOffset] _Colors ("Colors", 2D) = "white" {}
		_Emission ("Emission", Float) = 0
		_MetallicAngleSmoothStep ("MetallicAngleSmoothStep", Vector) = (0.4,0.75,0,0)
		[NoScaleOffset] _Normal_Map ("Normal Map", 2D) = "white" {}
		_Normal_Strength ("Normal Strength", Range(0, 1)) = 1
		_Normal_Scale ("Normal Scale", Float) = 0.2
		_Cookness ("Cookness", Range(0, 2)) = 0
		_CookColor ("CookColor", Vector) = (1,0.4874805,0,1)
		_BurntColor ("BurntColor", Vector) = (0,0,0,1)
		_Cooked_Normal_Strength ("Cooked Normal Strength", Range(0, 2)) = 0.7
		[ToggleUI] _Use_Skin ("Use Skin", Float) = 0
		[ToggleUI] _Rainbow_Skin ("Rainbow Skin", Float) = 0
		[KeywordEnum(Gradient Noise, Checkerboard, Voronoi)] _SKIN_TYPE ("Skin Type", Float) = 0
		[KeywordEnum(Only Metallic, Only Metallic Noise, Everything)] _SKIN_AFFECTS ("Skin Affects", Float) = 0
		_Metallic_Cutoff ("Metallic Cutoff", Range(0, 1)) = 0.5
		_Color_Offset ("Color Offset", Vector) = (0,0,0,0)
		_Color_Offset_2 ("Color Offset 2", Vector) = (0,0,0,0)
		_Skin_Smooth_Step ("Skin Smooth Step", Vector) = (0.5,0.5,0,0)
		_Skin_Noise_Scale ("Skin Noise Scale", Float) = 10
		_Noise_Rotation ("Noise Rotation", Float) = 45
		_UV_Rotation ("UV Rotation", Float) = 0
		_Skin_UV_Scale ("Skin UV Scale", Vector) = (1,1,0,0)
		_MetallicMetallicness ("MetallicMetallicness", Range(0, 1)) = 0
		_MetallicSmoothness ("MetallicSmoothness", Range(0, 1)) = 0
		_PlasticMetallicness ("PlasticMetallicness", Range(0, 1)) = 0
		_PlasticSmoothness ("PlasticSmoothness", Range(0, 1)) = 0
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	SubShader{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "RenderPipeline" = "UniversalPipeline" }

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			TEXTURE2D(_Colors);
			SAMPLER(sampler_Colors);

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float3 normalOS : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = TransformObjectToHClip(input.pos.xyz);
				output.normalWS = TransformObjectToWorldNormal(input.normalOS);
				output.uv = input.uv;
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				half4 baseColor = SAMPLE_TEXTURE2D(_Colors, sampler_Colors, input.uv);
				Light light = GetMainLight();
				half lighting = saturate(dot(normalize(input.normalWS), light.direction)) * 0.65h + 0.35h;
				return half4(baseColor.rgb * lighting, baseColor.a);
			}

			ENDHLSL
		}
	}
	Fallback "Universal Render Pipeline/Unlit"
}
