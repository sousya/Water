Shader "UI/URP_ImageOutline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _LineWidth("Line Width", Range(0, 10)) = 2
        _LineColor("Line Color", Color) = (1,1,1,1)
        [Toggle(USE_ALPHA_CLIP)] _UseAlphaClip("Use Alpha Clip", Float) = 0
        _AlphaThreshold("Alpha Threshold", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "OutlinePass"
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ USE_ALPHA_CLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float _LineWidth;
            float4 _LineColor;
            float _AlphaThreshold;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.position);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // »ù´¡ÑÕÉ«²ÉÑù
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                
                // Alpha²Ã¼ô¼ì²â
                #if USE_ALPHA_CLIP
                    clip(col.a - _AlphaThreshold);
                #endif

                // ¼ÆËã±ßÔµÆ«ÒÆ
                float2 offsets[8] = {
                    float2(-1,0), float2(1,0),
                    float2(0,1), float2(0,-1),
                    float2(-1,1), float2(1,1),
                    float2(-1,-1), float2(1,-1)
                };

                // ±ßÔµ¼ì²â
                half edge = 0;
                for(int j=0; j<8; j++)
                {
                    float2 uvOffset = i.uv + offsets[j] * _LineWidth * _MainTex_TexelSize.xy;
                    half4 neighbor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvOffset);
                    edge = max(edge, neighbor.a);
                }

                // »ìºÏÑÕÉ«
                half outlineAlpha = step(col.a, 0.9) * edge;
                half4 finalColor = lerp(col, _LineColor, outlineAlpha);
                finalColor.a = max(col.a, outlineAlpha);

                return finalColor;
            }
            ENDHLSL
        }
    }
}