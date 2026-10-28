Shader "Shader Graphs/CharacterShader" {
	Properties {
		[NoScaleOffset] _Colors ("Colors", 2D) = "white" {}
		[NoScaleOffset] _Normal_Map ("Normal Map", 2D) = "white" {}
		_Normal_Strength ("Normal Strength", Range(0, 1)) = 1
		_Normal_Scale ("Normal Scale", Float) = 0.2
		_SkinColor ("SkinColor", Vector) = (0,0,0,0)
		_PrimaryColor ("PrimaryColor", Vector) = (0,0,0,0)
		_SecondColor ("SecondColor", Vector) = (0,0,0,0)
		_ThirdColor ("ThirdColor", Vector) = (0,0,0,0)
		_NormalInfluence ("NormalInfluence", Float) = 0
		_SSSPower ("SSSPower", Float) = 0
		_SSSIntensity ("SSSIntensity", Float) = 0
		_SSSColor ("SSSColor", Vector) = (0,0,0,1)
		_Thickness ("Thickness", Float) = 0
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
				half lighting = saturate(dot(normalize(input.normalWS), light.direction)) * 0.6h + 0.4h;
				return half4(baseColor.rgb * lighting, baseColor.a);
			}

			ENDHLSL
		}
	}
	Fallback "Universal Render Pipeline/Unlit"
}
