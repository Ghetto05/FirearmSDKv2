Shader "Hidden/GlowingContour"
{
    Properties
    {
        _Intensity ("Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "EdgeDetection"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float _Intensity;

            half4 frag(Varyings input) : SV_Target
            {
                float2 texelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);

                half3 color = half3(0, 0, 0);
                color += abs(ddx(input.uv.x)) * _Intensity;
                color += abs(ddy(input.uv.y)) * _Intensity;

                return half4(color, 1);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Glow"
            ZTest Always Cull Off ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.vertex);
                output.uv = input.uv;
                return output;
            }

            float _Intensity;

            half4 frag(Varyings input) : SV_Target
            {
                half4 color = half4(0, 0, 0, 1);
                color.rgb += _Intensity;
                return color;
            }
            ENDHLSL
        }
    }
}
