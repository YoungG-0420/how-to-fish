Shader "HowToFish/Water"
{
    Properties
    {
        _BaseColor ("Water Color", Color) = (0.10, 0.61, 1.0, 0.58)
        _WaveStrength ("Wave Strength", Range(0, 0.25)) = 0.055
        _WaveScale ("Wave Scale", Range(0.1, 4)) = 0.7
        _WaveSpeed ("Wave Speed", Range(0, 3)) = 0.55
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "WaterForward"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float _WaveStrength;
                float _WaveScale;
                float _WaveSpeed;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float wave : TEXCOORD1; };

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                float waveA = sin((worldPos.x + _Time.y * _WaveSpeed) * _WaveScale);
                float waveB = cos((worldPos.z - _Time.y * _WaveSpeed * 0.8) * _WaveScale * 1.3);
                float wave = (waveA + waveB) * 0.5;
                worldPos.y += wave * _WaveStrength;
                output.positionCS = TransformWorldToHClip(worldPos);
                output.uv = input.uv;
                output.wave = wave;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half highlight = saturate(input.wave * 0.15h + 0.08h);
                half3 color = _BaseColor.rgb + half3(0.12h, 0.20h, 0.25h) * highlight;
                return half4(color, max(_BaseColor.a, 0.55h));
            }
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Unlit"
}
