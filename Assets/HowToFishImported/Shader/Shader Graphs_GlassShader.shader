Shader "Shader Graphs/GlassShader" {
	Properties {
		_Smooth_Step ("Smooth Step", Vector) = (0,0,0,0)
		_Distance_Min_Max ("Distance Min Max", Vector) = (1,5,0,0)
		[NoScaleOffset] _Colors ("Colors", 2D) = "white" {}
		_AlphaMax ("AlphaMax", Float) = 0
		_Normal_Scale ("Normal Scale", Float) = 0.3
		_Normal_Strength ("Normal Strength", Range(0, 1)) = 0.2
		[NoScaleOffset] _Normal_Texture ("Normal Texture", 2D) = "white" {}
		[HideInInspector] _WorkflowMode ("_WorkflowMode", Float) = 1
		[HideInInspector] _CastShadows ("_CastShadows", Float) = 1
		[HideInInspector] _ReceiveShadows ("_ReceiveShadows", Float) = 1
		[HideInInspector] _Surface ("_Surface", Float) = 1
		[HideInInspector] _Blend ("_Blend", Float) = 0
		[HideInInspector] _AlphaClip ("_AlphaClip", Float) = 0
		[HideInInspector] _BlendModePreserveSpecular ("_BlendModePreserveSpecular", Float) = 1
		[HideInInspector] _SrcBlend ("_SrcBlend", Float) = 1
		[HideInInspector] _DstBlend ("_DstBlend", Float) = 0
		[HideInInspector] _SrcBlendAlpha ("_SrcBlendAlpha", Float) = 1
		[HideInInspector] _DstBlendAlpha ("_DstBlendAlpha", Float) = 0
		[ToggleUI] [HideInInspector] _ZWrite ("_ZWrite", Float) = 0
		[HideInInspector] _ZWriteControl ("_ZWriteControl", Float) = 0
		[HideInInspector] _ZTest ("_ZTest", Float) = 4
		[HideInInspector] _Cull ("_Cull", Float) = 2
		[HideInInspector] _AlphaToMask ("_AlphaToMask", Float) = 0
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	SubShader{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			TEXTURE2D(_Colors);
			SAMPLER(sampler_Colors);
			float _AlphaMax;

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
				output.pos = TransformObjectToHClip(input.pos.xyz);
				output.uv = input.uv;
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				half4 c = SAMPLE_TEXTURE2D(_Colors, sampler_Colors, input.uv);
				c.a = max(0.12h, min(0.35h, _AlphaMax));
				return c;
			}

			ENDHLSL
		}
	}
	Fallback "Universal Render Pipeline/Unlit"
}
