Shader "FirearmSDKv2/Thermal/White Hot"
{
    Properties
    {
        _Temperature("Temperature", Float) = 45
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = "UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
            // LightMode: <None>
        }

        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest Always
        ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines

        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float3 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz = input.positionWS;
            output.interp1.xyz = input.normalWS;
            output.interp2.xyz = input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.viewDirectionWS = input.interp2.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }


        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _Temperature;
        CBUFFER_END

            // Object and Global properties
            Gradient _GradientStandard_Definition()
            {
                Gradient g;
                g.type = 0;
                g.colorsLength = 6;
                g.alphasLength = 2;
                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                g.colors[4] = float4(1, 0, 0, 0.9264668);
                g.colors[5] = float4(1, 1, 1, 1);
                g.colors[6] = float4(0, 0, 0, 0);
                g.colors[7] = float4(0, 0, 0, 0);
                g.alphas[0] = float2(1, 0);
                g.alphas[1] = float2(1, 1);
                g.alphas[2] = float2(0, 0);
                g.alphas[3] = float2(0, 0);
                g.alphas[4] = float2(0, 0);
                g.alphas[5] = float2(0, 0);
                g.alphas[6] = float2(0, 0);
                g.alphas[7] = float2(0, 0);
                return g;
            }
            #define _GradientStandard _GradientStandard_Definition()
            Gradient _GradientRedStrength_Definition()
            {
                Gradient g;
                g.type = 0;
                g.colorsLength = 5;
                g.alphasLength = 2;
                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                g.colors[3] = float4(1, 0, 0, 0.7794156);
                g.colors[4] = float4(1, 0, 0, 1);
                g.colors[5] = float4(0, 0, 0, 0);
                g.colors[6] = float4(0, 0, 0, 0);
                g.colors[7] = float4(0, 0, 0, 0);
                g.alphas[0] = float2(1, 0);
                g.alphas[1] = float2(1, 1);
                g.alphas[2] = float2(0, 0);
                g.alphas[3] = float2(0, 0);
                g.alphas[4] = float2(0, 0);
                g.alphas[5] = float2(0, 0);
                g.alphas[6] = float2(0, 0);
                g.alphas[7] = float2(0, 0);
                return g;
            }
            #define _GradientRedStrength _GradientRedStrength_Definition()
            Gradient _GradientWhiteHot_Definition()
            {
                Gradient g;
                g.type = 0;
                g.colorsLength = 3;
                g.alphasLength = 2;
                g.colors[0] = float4(0, 0, 0, 0);
                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                g.colors[2] = float4(1, 1, 1, 1);
                g.colors[3] = float4(0, 0, 0, 0);
                g.colors[4] = float4(0, 0, 0, 0);
                g.colors[5] = float4(0, 0, 0, 0);
                g.colors[6] = float4(0, 0, 0, 0);
                g.colors[7] = float4(0, 0, 0, 0);
                g.alphas[0] = float2(1, 0);
                g.alphas[1] = float2(1, 1);
                g.alphas[2] = float2(0, 0);
                g.alphas[3] = float2(0, 0);
                g.alphas[4] = float2(0, 0);
                g.alphas[5] = float2(0, 0);
                g.alphas[6] = float2(0, 0);
                g.alphas[7] = float2(0, 0);
                return g;
            }
            #define _GradientWhiteHot _GradientWhiteHot_Definition()
            Gradient _GradientBlackHot_Definition()
            {
                Gradient g;
                g.type = 0;
                g.colorsLength = 4;
                g.alphasLength = 2;
                g.colors[0] = float4(1, 1, 1, 0);
                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                g.colors[3] = float4(0, 0, 0, 1);
                g.colors[4] = float4(0, 0, 0, 0);
                g.colors[5] = float4(0, 0, 0, 0);
                g.colors[6] = float4(0, 0, 0, 0);
                g.colors[7] = float4(0, 0, 0, 0);
                g.alphas[0] = float2(1, 0);
                g.alphas[1] = float2(1, 1);
                g.alphas[2] = float2(0, 0);
                g.alphas[3] = float2(0, 0);
                g.alphas[4] = float2(0, 0);
                g.alphas[5] = float2(0, 0);
                g.alphas[6] = float2(0, 0);
                g.alphas[7] = float2(0, 0);
                return g;
            }
            #define _GradientBlackHot _GradientBlackHot_Definition()

            // Graph Includes
            // GraphIncludes: <None>

            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif

            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif

            // Graph Functions

            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }

            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }

            void Unity_InvertColors_float(float In, float InvertColors, out float Out)
            {
                Out = abs(InvertColors - In);
            }

            void Unity_Multiply_float_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
            {
                float3 color = Gradient.colors[0].rgb;
                [unroll]
                for (int c = 1; c < Gradient.colorsLength; c++)
                {
                    float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                    color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
                }
            #ifdef UNITY_COLORSPACE_GAMMA
                color = LinearToSRGB(color);
            #endif
                float alpha = Gradient.alphas[0].x;
                [unroll]
                for (int a = 1; a < Gradient.alphasLength; a++)
                {
                    float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                    alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
                }
                Out = float4(color, alpha);
            }

            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Custom interpolators, pre surface
            #ifdef FEATURES_GRAPH_VERTEX
            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
            {
            return output;
            }
            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
            #endif

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                Gradient _Property_ca36ee881dbd41e18ba6d0819a5219b2_Out_0 = _GradientWhiteHot;
                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                float _FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3;
                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, -0.4, _FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3);
                float _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1;
                float _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_InvertColors = float(1);
                Unity_InvertColors_float(_FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_InvertColors, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1);
                float _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2;
                Unity_Multiply_float_float(_Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1, _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2);
                float4 _SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2;
                Unity_SampleGradientV1_float(_Property_ca36ee881dbd41e18ba6d0819a5219b2_Out_0, _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2, _SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2);
                surface.BaseColor = (_SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2.xyz);
                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES Attributes
            #define VFX_SRP_VARYINGS Varyings
            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
            #endif
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

            #ifdef HAVE_VFX_MODIFICATION
                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

            #endif



                // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
                float3 unnormalizedNormalWS = input.normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);


                output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph


                output.WorldSpaceViewDirection = normalize(input.viewDirectionWS);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

            // --------------------------------------------------
            // Visual Effect Vertex Invocations
            #ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
            #endif

            ENDHLSL
            }
            Pass
            {
                Name "DepthNormalsOnly"
                Tags
                {
                    "LightMode" = "DepthNormalsOnly"
                }

                // Render State
                Cull Back
                ZTest Always
                ZWrite On

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                HLSLPROGRAM

                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag

                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines

                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                // custom interpolator pre-include
                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                // --------------------------------------------------
                // Structs and Packing

                // custom interpolators pre packing
                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                struct Attributes
                {
                     float3 positionOS : POSITION;
                     float3 normalOS : NORMAL;
                     float4 tangentOS : TANGENT;
                     float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                     float4 positionCS : SV_POSITION;
                     float3 normalWS;
                     float4 tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                };
                struct VertexDescriptionInputs
                {
                     float3 ObjectSpaceNormal;
                     float3 ObjectSpaceTangent;
                     float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                     float4 positionCS : SV_POSITION;
                     float3 interp0 : INTERP0;
                     float4 interp1 : INTERP1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output;
                    ZERO_INITIALIZE(PackedVaryings, output);
                    output.positionCS = input.positionCS;
                    output.interp0.xyz = input.normalWS;
                    output.interp1.xyzw = input.tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.normalWS = input.interp0.xyz;
                    output.tangentWS = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }


                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float _Temperature;
                CBUFFER_END

                    // Object and Global properties
                    Gradient _GradientStandard_Definition()
                    {
                        Gradient g;
                        g.type = 0;
                        g.colorsLength = 6;
                        g.alphasLength = 2;
                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                        g.colors[5] = float4(1, 1, 1, 1);
                        g.colors[6] = float4(0, 0, 0, 0);
                        g.colors[7] = float4(0, 0, 0, 0);
                        g.alphas[0] = float2(1, 0);
                        g.alphas[1] = float2(1, 1);
                        g.alphas[2] = float2(0, 0);
                        g.alphas[3] = float2(0, 0);
                        g.alphas[4] = float2(0, 0);
                        g.alphas[5] = float2(0, 0);
                        g.alphas[6] = float2(0, 0);
                        g.alphas[7] = float2(0, 0);
                        return g;
                    }
                    #define _GradientStandard _GradientStandard_Definition()
                    Gradient _GradientRedStrength_Definition()
                    {
                        Gradient g;
                        g.type = 0;
                        g.colorsLength = 5;
                        g.alphasLength = 2;
                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                        g.colors[4] = float4(1, 0, 0, 1);
                        g.colors[5] = float4(0, 0, 0, 0);
                        g.colors[6] = float4(0, 0, 0, 0);
                        g.colors[7] = float4(0, 0, 0, 0);
                        g.alphas[0] = float2(1, 0);
                        g.alphas[1] = float2(1, 1);
                        g.alphas[2] = float2(0, 0);
                        g.alphas[3] = float2(0, 0);
                        g.alphas[4] = float2(0, 0);
                        g.alphas[5] = float2(0, 0);
                        g.alphas[6] = float2(0, 0);
                        g.alphas[7] = float2(0, 0);
                        return g;
                    }
                    #define _GradientRedStrength _GradientRedStrength_Definition()
                    Gradient _GradientWhiteHot_Definition()
                    {
                        Gradient g;
                        g.type = 0;
                        g.colorsLength = 3;
                        g.alphasLength = 2;
                        g.colors[0] = float4(0, 0, 0, 0);
                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                        g.colors[2] = float4(1, 1, 1, 1);
                        g.colors[3] = float4(0, 0, 0, 0);
                        g.colors[4] = float4(0, 0, 0, 0);
                        g.colors[5] = float4(0, 0, 0, 0);
                        g.colors[6] = float4(0, 0, 0, 0);
                        g.colors[7] = float4(0, 0, 0, 0);
                        g.alphas[0] = float2(1, 0);
                        g.alphas[1] = float2(1, 1);
                        g.alphas[2] = float2(0, 0);
                        g.alphas[3] = float2(0, 0);
                        g.alphas[4] = float2(0, 0);
                        g.alphas[5] = float2(0, 0);
                        g.alphas[6] = float2(0, 0);
                        g.alphas[7] = float2(0, 0);
                        return g;
                    }
                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                    Gradient _GradientBlackHot_Definition()
                    {
                        Gradient g;
                        g.type = 0;
                        g.colorsLength = 4;
                        g.alphasLength = 2;
                        g.colors[0] = float4(1, 1, 1, 0);
                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                        g.colors[3] = float4(0, 0, 0, 1);
                        g.colors[4] = float4(0, 0, 0, 0);
                        g.colors[5] = float4(0, 0, 0, 0);
                        g.colors[6] = float4(0, 0, 0, 0);
                        g.colors[7] = float4(0, 0, 0, 0);
                        g.alphas[0] = float2(1, 0);
                        g.alphas[1] = float2(1, 1);
                        g.alphas[2] = float2(0, 0);
                        g.alphas[3] = float2(0, 0);
                        g.alphas[4] = float2(0, 0);
                        g.alphas[5] = float2(0, 0);
                        g.alphas[6] = float2(0, 0);
                        g.alphas[7] = float2(0, 0);
                        return g;
                    }
                    #define _GradientBlackHot _GradientBlackHot_Definition()

                    // Graph Includes
                    // GraphIncludes: <None>

                    // -- Property used by ScenePickingPass
                    #ifdef SCENEPICKINGPASS
                    float4 _SelectionID;
                    #endif

                    // -- Properties used by SceneSelectionPass
                    #ifdef SCENESELECTIONPASS
                    int _ObjectId;
                    int _PassValue;
                    #endif

                    // Graph Functions

                    void Unity_Divide_float(float A, float B, out float Out)
                    {
                        Out = A / B;
                    }

                    // Custom interpolators pre vertex
                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                    // Graph Vertex
                    struct VertexDescription
                    {
                        float3 Position;
                        float3 Normal;
                        float3 Tangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        description.Position = IN.ObjectSpacePosition;
                        description.Normal = IN.ObjectSpaceNormal;
                        description.Tangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Custom interpolators, pre surface
                    #ifdef FEATURES_GRAPH_VERTEX
                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                    {
                    return output;
                    }
                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                    #endif

                    // Graph Pixel
                    struct SurfaceDescription
                    {
                        float Alpha;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs
                    #ifdef HAVE_VFX_MODIFICATION
                    #define VFX_SRP_ATTRIBUTES Attributes
                    #define VFX_SRP_VARYINGS Varyings
                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                    #endif
                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        output.ObjectSpaceNormal = input.normalOS;
                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                        output.ObjectSpacePosition = input.positionOS;

                        return output;
                    }
                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                    #ifdef HAVE_VFX_MODIFICATION
                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                    #endif







                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                    }

                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                    // --------------------------------------------------
                    // Visual Effect Vertex Invocations
                    #ifdef HAVE_VFX_MODIFICATION
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                    #endif

                    ENDHLSL
                    }
                    Pass
                    {
                        Name "ShadowCaster"
                        Tags
                        {
                            "LightMode" = "ShadowCaster"
                        }

                        // Render State
                        Cull Back
                        ZTest Always
                        ZWrite On
                        ColorMask 0

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        HLSLPROGRAM

                        // Pragmas
                        #pragma target 4.5
                        #pragma exclude_renderers gles gles3 glcore
                        #pragma multi_compile_instancing
                        #pragma multi_compile _ DOTS_INSTANCING_ON
                        #pragma vertex vert
                        #pragma fragment frag

                        // DotsInstancingOptions: <None>
                        // HybridV1InjectedBuiltinProperties: <None>

                        // Keywords
                        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
                        // GraphKeywords: <None>

                        // Defines

                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define VARYINGS_NEED_NORMAL_WS
                        #define FEATURES_GRAPH_VERTEX
                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                        #define SHADERPASS SHADERPASS_SHADOWCASTER
                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                        // custom interpolator pre-include
                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                        // --------------------------------------------------
                        // Structs and Packing

                        // custom interpolators pre packing
                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                        struct Attributes
                        {
                             float3 positionOS : POSITION;
                             float3 normalOS : NORMAL;
                             float4 tangentOS : TANGENT;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };
                        struct Varyings
                        {
                             float4 positionCS : SV_POSITION;
                             float3 normalWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };
                        struct SurfaceDescriptionInputs
                        {
                        };
                        struct VertexDescriptionInputs
                        {
                             float3 ObjectSpaceNormal;
                             float3 ObjectSpaceTangent;
                             float3 ObjectSpacePosition;
                        };
                        struct PackedVaryings
                        {
                             float4 positionCS : SV_POSITION;
                             float3 interp0 : INTERP0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                             uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output;
                            ZERO_INITIALIZE(PackedVaryings, output);
                            output.positionCS = input.positionCS;
                            output.interp0.xyz = input.normalWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output;
                            output.positionCS = input.positionCS;
                            output.normalWS = input.interp0.xyz;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }


                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float _Temperature;
                        CBUFFER_END

                            // Object and Global properties
                            Gradient _GradientStandard_Definition()
                            {
                                Gradient g;
                                g.type = 0;
                                g.colorsLength = 6;
                                g.alphasLength = 2;
                                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                g.colors[4] = float4(1, 0, 0, 0.9264668);
                                g.colors[5] = float4(1, 1, 1, 1);
                                g.colors[6] = float4(0, 0, 0, 0);
                                g.colors[7] = float4(0, 0, 0, 0);
                                g.alphas[0] = float2(1, 0);
                                g.alphas[1] = float2(1, 1);
                                g.alphas[2] = float2(0, 0);
                                g.alphas[3] = float2(0, 0);
                                g.alphas[4] = float2(0, 0);
                                g.alphas[5] = float2(0, 0);
                                g.alphas[6] = float2(0, 0);
                                g.alphas[7] = float2(0, 0);
                                return g;
                            }
                            #define _GradientStandard _GradientStandard_Definition()
                            Gradient _GradientRedStrength_Definition()
                            {
                                Gradient g;
                                g.type = 0;
                                g.colorsLength = 5;
                                g.alphasLength = 2;
                                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                g.colors[3] = float4(1, 0, 0, 0.7794156);
                                g.colors[4] = float4(1, 0, 0, 1);
                                g.colors[5] = float4(0, 0, 0, 0);
                                g.colors[6] = float4(0, 0, 0, 0);
                                g.colors[7] = float4(0, 0, 0, 0);
                                g.alphas[0] = float2(1, 0);
                                g.alphas[1] = float2(1, 1);
                                g.alphas[2] = float2(0, 0);
                                g.alphas[3] = float2(0, 0);
                                g.alphas[4] = float2(0, 0);
                                g.alphas[5] = float2(0, 0);
                                g.alphas[6] = float2(0, 0);
                                g.alphas[7] = float2(0, 0);
                                return g;
                            }
                            #define _GradientRedStrength _GradientRedStrength_Definition()
                            Gradient _GradientWhiteHot_Definition()
                            {
                                Gradient g;
                                g.type = 0;
                                g.colorsLength = 3;
                                g.alphasLength = 2;
                                g.colors[0] = float4(0, 0, 0, 0);
                                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                g.colors[2] = float4(1, 1, 1, 1);
                                g.colors[3] = float4(0, 0, 0, 0);
                                g.colors[4] = float4(0, 0, 0, 0);
                                g.colors[5] = float4(0, 0, 0, 0);
                                g.colors[6] = float4(0, 0, 0, 0);
                                g.colors[7] = float4(0, 0, 0, 0);
                                g.alphas[0] = float2(1, 0);
                                g.alphas[1] = float2(1, 1);
                                g.alphas[2] = float2(0, 0);
                                g.alphas[3] = float2(0, 0);
                                g.alphas[4] = float2(0, 0);
                                g.alphas[5] = float2(0, 0);
                                g.alphas[6] = float2(0, 0);
                                g.alphas[7] = float2(0, 0);
                                return g;
                            }
                            #define _GradientWhiteHot _GradientWhiteHot_Definition()
                            Gradient _GradientBlackHot_Definition()
                            {
                                Gradient g;
                                g.type = 0;
                                g.colorsLength = 4;
                                g.alphasLength = 2;
                                g.colors[0] = float4(1, 1, 1, 0);
                                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                g.colors[3] = float4(0, 0, 0, 1);
                                g.colors[4] = float4(0, 0, 0, 0);
                                g.colors[5] = float4(0, 0, 0, 0);
                                g.colors[6] = float4(0, 0, 0, 0);
                                g.colors[7] = float4(0, 0, 0, 0);
                                g.alphas[0] = float2(1, 0);
                                g.alphas[1] = float2(1, 1);
                                g.alphas[2] = float2(0, 0);
                                g.alphas[3] = float2(0, 0);
                                g.alphas[4] = float2(0, 0);
                                g.alphas[5] = float2(0, 0);
                                g.alphas[6] = float2(0, 0);
                                g.alphas[7] = float2(0, 0);
                                return g;
                            }
                            #define _GradientBlackHot _GradientBlackHot_Definition()

                            // Graph Includes
                            // GraphIncludes: <None>

                            // -- Property used by ScenePickingPass
                            #ifdef SCENEPICKINGPASS
                            float4 _SelectionID;
                            #endif

                            // -- Properties used by SceneSelectionPass
                            #ifdef SCENESELECTIONPASS
                            int _ObjectId;
                            int _PassValue;
                            #endif

                            // Graph Functions

                            void Unity_Divide_float(float A, float B, out float Out)
                            {
                                Out = A / B;
                            }

                            // Custom interpolators pre vertex
                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                            // Graph Vertex
                            struct VertexDescription
                            {
                                float3 Position;
                                float3 Normal;
                                float3 Tangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                description.Position = IN.ObjectSpacePosition;
                                description.Normal = IN.ObjectSpaceNormal;
                                description.Tangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Custom interpolators, pre surface
                            #ifdef FEATURES_GRAPH_VERTEX
                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                            {
                            return output;
                            }
                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                            #endif

                            // Graph Pixel
                            struct SurfaceDescription
                            {
                                float Alpha;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs
                            #ifdef HAVE_VFX_MODIFICATION
                            #define VFX_SRP_ATTRIBUTES Attributes
                            #define VFX_SRP_VARYINGS Varyings
                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                            #endif
                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                output.ObjectSpaceNormal = input.normalOS;
                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                output.ObjectSpacePosition = input.positionOS;

                                return output;
                            }
                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                            #ifdef HAVE_VFX_MODIFICATION
                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                            #endif







                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                    return output;
                            }

                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                            // --------------------------------------------------
                            // Visual Effect Vertex Invocations
                            #ifdef HAVE_VFX_MODIFICATION
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                            #endif

                            ENDHLSL
                            }
                            Pass
                            {
                                Name "SceneSelectionPass"
                                Tags
                                {
                                    "LightMode" = "SceneSelectionPass"
                                }

                                // Render State
                                Cull Off

                                // Debug
                                // <None>

                                // --------------------------------------------------
                                // Pass

                                HLSLPROGRAM

                                // Pragmas
                                #pragma target 4.5
                                #pragma exclude_renderers gles gles3 glcore
                                #pragma vertex vert
                                #pragma fragment frag

                                // DotsInstancingOptions: <None>
                                // HybridV1InjectedBuiltinProperties: <None>

                                // Keywords
                                // PassKeywords: <None>
                                // GraphKeywords: <None>

                                // Defines

                                #define ATTRIBUTES_NEED_NORMAL
                                #define ATTRIBUTES_NEED_TANGENT
                                #define FEATURES_GRAPH_VERTEX
                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                #define SHADERPASS SHADERPASS_DEPTHONLY
                                #define SCENESELECTIONPASS 1
                                #define ALPHA_CLIP_THRESHOLD 1
                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                // custom interpolator pre-include
                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                // Includes
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                // --------------------------------------------------
                                // Structs and Packing

                                // custom interpolators pre packing
                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                struct Attributes
                                {
                                     float3 positionOS : POSITION;
                                     float3 normalOS : NORMAL;
                                     float4 tangentOS : TANGENT;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : INSTANCEID_SEMANTIC;
                                    #endif
                                };
                                struct Varyings
                                {
                                     float4 positionCS : SV_POSITION;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };
                                struct SurfaceDescriptionInputs
                                {
                                };
                                struct VertexDescriptionInputs
                                {
                                     float3 ObjectSpaceNormal;
                                     float3 ObjectSpaceTangent;
                                     float3 ObjectSpacePosition;
                                };
                                struct PackedVaryings
                                {
                                     float4 positionCS : SV_POSITION;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };

                                PackedVaryings PackVaryings(Varyings input)
                                {
                                    PackedVaryings output;
                                    ZERO_INITIALIZE(PackedVaryings, output);
                                    output.positionCS = input.positionCS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }

                                Varyings UnpackVaryings(PackedVaryings input)
                                {
                                    Varyings output;
                                    output.positionCS = input.positionCS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }


                                // --------------------------------------------------
                                // Graph

                                // Graph Properties
                                CBUFFER_START(UnityPerMaterial)
                                float _Temperature;
                                CBUFFER_END

                                    // Object and Global properties
                                    Gradient _GradientStandard_Definition()
                                    {
                                        Gradient g;
                                        g.type = 0;
                                        g.colorsLength = 6;
                                        g.alphasLength = 2;
                                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                                        g.colors[5] = float4(1, 1, 1, 1);
                                        g.colors[6] = float4(0, 0, 0, 0);
                                        g.colors[7] = float4(0, 0, 0, 0);
                                        g.alphas[0] = float2(1, 0);
                                        g.alphas[1] = float2(1, 1);
                                        g.alphas[2] = float2(0, 0);
                                        g.alphas[3] = float2(0, 0);
                                        g.alphas[4] = float2(0, 0);
                                        g.alphas[5] = float2(0, 0);
                                        g.alphas[6] = float2(0, 0);
                                        g.alphas[7] = float2(0, 0);
                                        return g;
                                    }
                                    #define _GradientStandard _GradientStandard_Definition()
                                    Gradient _GradientRedStrength_Definition()
                                    {
                                        Gradient g;
                                        g.type = 0;
                                        g.colorsLength = 5;
                                        g.alphasLength = 2;
                                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                                        g.colors[4] = float4(1, 0, 0, 1);
                                        g.colors[5] = float4(0, 0, 0, 0);
                                        g.colors[6] = float4(0, 0, 0, 0);
                                        g.colors[7] = float4(0, 0, 0, 0);
                                        g.alphas[0] = float2(1, 0);
                                        g.alphas[1] = float2(1, 1);
                                        g.alphas[2] = float2(0, 0);
                                        g.alphas[3] = float2(0, 0);
                                        g.alphas[4] = float2(0, 0);
                                        g.alphas[5] = float2(0, 0);
                                        g.alphas[6] = float2(0, 0);
                                        g.alphas[7] = float2(0, 0);
                                        return g;
                                    }
                                    #define _GradientRedStrength _GradientRedStrength_Definition()
                                    Gradient _GradientWhiteHot_Definition()
                                    {
                                        Gradient g;
                                        g.type = 0;
                                        g.colorsLength = 3;
                                        g.alphasLength = 2;
                                        g.colors[0] = float4(0, 0, 0, 0);
                                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                        g.colors[2] = float4(1, 1, 1, 1);
                                        g.colors[3] = float4(0, 0, 0, 0);
                                        g.colors[4] = float4(0, 0, 0, 0);
                                        g.colors[5] = float4(0, 0, 0, 0);
                                        g.colors[6] = float4(0, 0, 0, 0);
                                        g.colors[7] = float4(0, 0, 0, 0);
                                        g.alphas[0] = float2(1, 0);
                                        g.alphas[1] = float2(1, 1);
                                        g.alphas[2] = float2(0, 0);
                                        g.alphas[3] = float2(0, 0);
                                        g.alphas[4] = float2(0, 0);
                                        g.alphas[5] = float2(0, 0);
                                        g.alphas[6] = float2(0, 0);
                                        g.alphas[7] = float2(0, 0);
                                        return g;
                                    }
                                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                    Gradient _GradientBlackHot_Definition()
                                    {
                                        Gradient g;
                                        g.type = 0;
                                        g.colorsLength = 4;
                                        g.alphasLength = 2;
                                        g.colors[0] = float4(1, 1, 1, 0);
                                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                        g.colors[3] = float4(0, 0, 0, 1);
                                        g.colors[4] = float4(0, 0, 0, 0);
                                        g.colors[5] = float4(0, 0, 0, 0);
                                        g.colors[6] = float4(0, 0, 0, 0);
                                        g.colors[7] = float4(0, 0, 0, 0);
                                        g.alphas[0] = float2(1, 0);
                                        g.alphas[1] = float2(1, 1);
                                        g.alphas[2] = float2(0, 0);
                                        g.alphas[3] = float2(0, 0);
                                        g.alphas[4] = float2(0, 0);
                                        g.alphas[5] = float2(0, 0);
                                        g.alphas[6] = float2(0, 0);
                                        g.alphas[7] = float2(0, 0);
                                        return g;
                                    }
                                    #define _GradientBlackHot _GradientBlackHot_Definition()

                                    // Graph Includes
                                    // GraphIncludes: <None>

                                    // -- Property used by ScenePickingPass
                                    #ifdef SCENEPICKINGPASS
                                    float4 _SelectionID;
                                    #endif

                                    // -- Properties used by SceneSelectionPass
                                    #ifdef SCENESELECTIONPASS
                                    int _ObjectId;
                                    int _PassValue;
                                    #endif

                                    // Graph Functions

                                    void Unity_Divide_float(float A, float B, out float Out)
                                    {
                                        Out = A / B;
                                    }

                                    // Custom interpolators pre vertex
                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                    // Graph Vertex
                                    struct VertexDescription
                                    {
                                        float3 Position;
                                        float3 Normal;
                                        float3 Tangent;
                                    };

                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                    {
                                        VertexDescription description = (VertexDescription)0;
                                        description.Position = IN.ObjectSpacePosition;
                                        description.Normal = IN.ObjectSpaceNormal;
                                        description.Tangent = IN.ObjectSpaceTangent;
                                        return description;
                                    }

                                    // Custom interpolators, pre surface
                                    #ifdef FEATURES_GRAPH_VERTEX
                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                    {
                                    return output;
                                    }
                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                    #endif

                                    // Graph Pixel
                                    struct SurfaceDescription
                                    {
                                        float Alpha;
                                    };

                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                    {
                                        SurfaceDescription surface = (SurfaceDescription)0;
                                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                        return surface;
                                    }

                                    // --------------------------------------------------
                                    // Build Graph Inputs
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #define VFX_SRP_ATTRIBUTES Attributes
                                    #define VFX_SRP_VARYINGS Varyings
                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                    #endif
                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                    {
                                        VertexDescriptionInputs output;
                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                        output.ObjectSpaceNormal = input.normalOS;
                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                        output.ObjectSpacePosition = input.positionOS;

                                        return output;
                                    }
                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                    {
                                        SurfaceDescriptionInputs output;
                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                    #ifdef HAVE_VFX_MODIFICATION
                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                    #endif







                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                    #else
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                    #endif
                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                            return output;
                                    }

                                    // --------------------------------------------------
                                    // Main

                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                                    // --------------------------------------------------
                                    // Visual Effect Vertex Invocations
                                    #ifdef HAVE_VFX_MODIFICATION
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                    #endif

                                    ENDHLSL
                                    }
                                    Pass
                                    {
                                        Name "ScenePickingPass"
                                        Tags
                                        {
                                            "LightMode" = "Picking"
                                        }

                                        // Render State
                                        Cull Back

                                        // Debug
                                        // <None>

                                        // --------------------------------------------------
                                        // Pass

                                        HLSLPROGRAM

                                        // Pragmas
                                        #pragma target 4.5
                                        #pragma exclude_renderers gles gles3 glcore
                                        #pragma vertex vert
                                        #pragma fragment frag

                                        // DotsInstancingOptions: <None>
                                        // HybridV1InjectedBuiltinProperties: <None>

                                        // Keywords
                                        // PassKeywords: <None>
                                        // GraphKeywords: <None>

                                        // Defines

                                        #define ATTRIBUTES_NEED_NORMAL
                                        #define ATTRIBUTES_NEED_TANGENT
                                        #define FEATURES_GRAPH_VERTEX
                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                        #define SHADERPASS SHADERPASS_DEPTHONLY
                                        #define SCENEPICKINGPASS 1
                                        #define ALPHA_CLIP_THRESHOLD 1
                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                        // custom interpolator pre-include
                                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                        // Includes
                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                        // --------------------------------------------------
                                        // Structs and Packing

                                        // custom interpolators pre packing
                                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                        struct Attributes
                                        {
                                             float3 positionOS : POSITION;
                                             float3 normalOS : NORMAL;
                                             float4 tangentOS : TANGENT;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                             uint instanceID : INSTANCEID_SEMANTIC;
                                            #endif
                                        };
                                        struct Varyings
                                        {
                                             float4 positionCS : SV_POSITION;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                            #endif
                                        };
                                        struct SurfaceDescriptionInputs
                                        {
                                        };
                                        struct VertexDescriptionInputs
                                        {
                                             float3 ObjectSpaceNormal;
                                             float3 ObjectSpaceTangent;
                                             float3 ObjectSpacePosition;
                                        };
                                        struct PackedVaryings
                                        {
                                             float4 positionCS : SV_POSITION;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                            #endif
                                        };

                                        PackedVaryings PackVaryings(Varyings input)
                                        {
                                            PackedVaryings output;
                                            ZERO_INITIALIZE(PackedVaryings, output);
                                            output.positionCS = input.positionCS;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            output.instanceID = input.instanceID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            output.cullFace = input.cullFace;
                                            #endif
                                            return output;
                                        }

                                        Varyings UnpackVaryings(PackedVaryings input)
                                        {
                                            Varyings output;
                                            output.positionCS = input.positionCS;
                                            #if UNITY_ANY_INSTANCING_ENABLED
                                            output.instanceID = input.instanceID;
                                            #endif
                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                            #endif
                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                            #endif
                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            output.cullFace = input.cullFace;
                                            #endif
                                            return output;
                                        }


                                        // --------------------------------------------------
                                        // Graph

                                        // Graph Properties
                                        CBUFFER_START(UnityPerMaterial)
                                        float _Temperature;
                                        CBUFFER_END

                                            // Object and Global properties
                                            Gradient _GradientStandard_Definition()
                                            {
                                                Gradient g;
                                                g.type = 0;
                                                g.colorsLength = 6;
                                                g.alphasLength = 2;
                                                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                g.colors[5] = float4(1, 1, 1, 1);
                                                g.colors[6] = float4(0, 0, 0, 0);
                                                g.colors[7] = float4(0, 0, 0, 0);
                                                g.alphas[0] = float2(1, 0);
                                                g.alphas[1] = float2(1, 1);
                                                g.alphas[2] = float2(0, 0);
                                                g.alphas[3] = float2(0, 0);
                                                g.alphas[4] = float2(0, 0);
                                                g.alphas[5] = float2(0, 0);
                                                g.alphas[6] = float2(0, 0);
                                                g.alphas[7] = float2(0, 0);
                                                return g;
                                            }
                                            #define _GradientStandard _GradientStandard_Definition()
                                            Gradient _GradientRedStrength_Definition()
                                            {
                                                Gradient g;
                                                g.type = 0;
                                                g.colorsLength = 5;
                                                g.alphasLength = 2;
                                                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                g.colors[4] = float4(1, 0, 0, 1);
                                                g.colors[5] = float4(0, 0, 0, 0);
                                                g.colors[6] = float4(0, 0, 0, 0);
                                                g.colors[7] = float4(0, 0, 0, 0);
                                                g.alphas[0] = float2(1, 0);
                                                g.alphas[1] = float2(1, 1);
                                                g.alphas[2] = float2(0, 0);
                                                g.alphas[3] = float2(0, 0);
                                                g.alphas[4] = float2(0, 0);
                                                g.alphas[5] = float2(0, 0);
                                                g.alphas[6] = float2(0, 0);
                                                g.alphas[7] = float2(0, 0);
                                                return g;
                                            }
                                            #define _GradientRedStrength _GradientRedStrength_Definition()
                                            Gradient _GradientWhiteHot_Definition()
                                            {
                                                Gradient g;
                                                g.type = 0;
                                                g.colorsLength = 3;
                                                g.alphasLength = 2;
                                                g.colors[0] = float4(0, 0, 0, 0);
                                                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                g.colors[2] = float4(1, 1, 1, 1);
                                                g.colors[3] = float4(0, 0, 0, 0);
                                                g.colors[4] = float4(0, 0, 0, 0);
                                                g.colors[5] = float4(0, 0, 0, 0);
                                                g.colors[6] = float4(0, 0, 0, 0);
                                                g.colors[7] = float4(0, 0, 0, 0);
                                                g.alphas[0] = float2(1, 0);
                                                g.alphas[1] = float2(1, 1);
                                                g.alphas[2] = float2(0, 0);
                                                g.alphas[3] = float2(0, 0);
                                                g.alphas[4] = float2(0, 0);
                                                g.alphas[5] = float2(0, 0);
                                                g.alphas[6] = float2(0, 0);
                                                g.alphas[7] = float2(0, 0);
                                                return g;
                                            }
                                            #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                            Gradient _GradientBlackHot_Definition()
                                            {
                                                Gradient g;
                                                g.type = 0;
                                                g.colorsLength = 4;
                                                g.alphasLength = 2;
                                                g.colors[0] = float4(1, 1, 1, 0);
                                                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                g.colors[3] = float4(0, 0, 0, 1);
                                                g.colors[4] = float4(0, 0, 0, 0);
                                                g.colors[5] = float4(0, 0, 0, 0);
                                                g.colors[6] = float4(0, 0, 0, 0);
                                                g.colors[7] = float4(0, 0, 0, 0);
                                                g.alphas[0] = float2(1, 0);
                                                g.alphas[1] = float2(1, 1);
                                                g.alphas[2] = float2(0, 0);
                                                g.alphas[3] = float2(0, 0);
                                                g.alphas[4] = float2(0, 0);
                                                g.alphas[5] = float2(0, 0);
                                                g.alphas[6] = float2(0, 0);
                                                g.alphas[7] = float2(0, 0);
                                                return g;
                                            }
                                            #define _GradientBlackHot _GradientBlackHot_Definition()

                                            // Graph Includes
                                            // GraphIncludes: <None>

                                            // -- Property used by ScenePickingPass
                                            #ifdef SCENEPICKINGPASS
                                            float4 _SelectionID;
                                            #endif

                                            // -- Properties used by SceneSelectionPass
                                            #ifdef SCENESELECTIONPASS
                                            int _ObjectId;
                                            int _PassValue;
                                            #endif

                                            // Graph Functions

                                            void Unity_Divide_float(float A, float B, out float Out)
                                            {
                                                Out = A / B;
                                            }

                                            // Custom interpolators pre vertex
                                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                            // Graph Vertex
                                            struct VertexDescription
                                            {
                                                float3 Position;
                                                float3 Normal;
                                                float3 Tangent;
                                            };

                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                            {
                                                VertexDescription description = (VertexDescription)0;
                                                description.Position = IN.ObjectSpacePosition;
                                                description.Normal = IN.ObjectSpaceNormal;
                                                description.Tangent = IN.ObjectSpaceTangent;
                                                return description;
                                            }

                                            // Custom interpolators, pre surface
                                            #ifdef FEATURES_GRAPH_VERTEX
                                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                            {
                                            return output;
                                            }
                                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                            #endif

                                            // Graph Pixel
                                            struct SurfaceDescription
                                            {
                                                float Alpha;
                                            };

                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                            {
                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                return surface;
                                            }

                                            // --------------------------------------------------
                                            // Build Graph Inputs
                                            #ifdef HAVE_VFX_MODIFICATION
                                            #define VFX_SRP_ATTRIBUTES Attributes
                                            #define VFX_SRP_VARYINGS Varyings
                                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                            #endif
                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                            {
                                                VertexDescriptionInputs output;
                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                output.ObjectSpaceNormal = input.normalOS;
                                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                output.ObjectSpacePosition = input.positionOS;

                                                return output;
                                            }
                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                            {
                                                SurfaceDescriptionInputs output;
                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                            #ifdef HAVE_VFX_MODIFICATION
                                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                            #endif







                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                            #else
                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                            #endif
                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                    return output;
                                            }

                                            // --------------------------------------------------
                                            // Main

                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                                            // --------------------------------------------------
                                            // Visual Effect Vertex Invocations
                                            #ifdef HAVE_VFX_MODIFICATION
                                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                            #endif

                                            ENDHLSL
                                            }
                                            Pass
                                            {
                                                Name "DepthNormals"
                                                Tags
                                                {
                                                    "LightMode" = "DepthNormalsOnly"
                                                }

                                                // Render State
                                                Cull Back
                                                ZTest Always
                                                ZWrite On

                                                // Debug
                                                // <None>

                                                // --------------------------------------------------
                                                // Pass

                                                HLSLPROGRAM

                                                // Pragmas
                                                #pragma target 4.5
                                                #pragma exclude_renderers gles gles3 glcore
                                                #pragma multi_compile_instancing
                                                #pragma multi_compile _ DOTS_INSTANCING_ON
                                                #pragma vertex vert
                                                #pragma fragment frag

                                                // DotsInstancingOptions: <None>
                                                // HybridV1InjectedBuiltinProperties: <None>

                                                // Keywords
                                                // PassKeywords: <None>
                                                // GraphKeywords: <None>

                                                // Defines

                                                #define ATTRIBUTES_NEED_NORMAL
                                                #define ATTRIBUTES_NEED_TANGENT
                                                #define VARYINGS_NEED_NORMAL_WS
                                                #define FEATURES_GRAPH_VERTEX
                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                // custom interpolator pre-include
                                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                // Includes
                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                // --------------------------------------------------
                                                // Structs and Packing

                                                // custom interpolators pre packing
                                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                struct Attributes
                                                {
                                                     float3 positionOS : POSITION;
                                                     float3 normalOS : NORMAL;
                                                     float4 tangentOS : TANGENT;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                     uint instanceID : INSTANCEID_SEMANTIC;
                                                    #endif
                                                };
                                                struct Varyings
                                                {
                                                     float4 positionCS : SV_POSITION;
                                                     float3 normalWS;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                    #endif
                                                };
                                                struct SurfaceDescriptionInputs
                                                {
                                                };
                                                struct VertexDescriptionInputs
                                                {
                                                     float3 ObjectSpaceNormal;
                                                     float3 ObjectSpaceTangent;
                                                     float3 ObjectSpacePosition;
                                                };
                                                struct PackedVaryings
                                                {
                                                     float4 positionCS : SV_POSITION;
                                                     float3 interp0 : INTERP0;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                    #endif
                                                };

                                                PackedVaryings PackVaryings(Varyings input)
                                                {
                                                    PackedVaryings output;
                                                    ZERO_INITIALIZE(PackedVaryings, output);
                                                    output.positionCS = input.positionCS;
                                                    output.interp0.xyz = input.normalWS;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    output.instanceID = input.instanceID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    output.cullFace = input.cullFace;
                                                    #endif
                                                    return output;
                                                }

                                                Varyings UnpackVaryings(PackedVaryings input)
                                                {
                                                    Varyings output;
                                                    output.positionCS = input.positionCS;
                                                    output.normalWS = input.interp0.xyz;
                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                    output.instanceID = input.instanceID;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                    #endif
                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                    #endif
                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    output.cullFace = input.cullFace;
                                                    #endif
                                                    return output;
                                                }


                                                // --------------------------------------------------
                                                // Graph

                                                // Graph Properties
                                                CBUFFER_START(UnityPerMaterial)
                                                float _Temperature;
                                                CBUFFER_END

                                                    // Object and Global properties
                                                    Gradient _GradientStandard_Definition()
                                                    {
                                                        Gradient g;
                                                        g.type = 0;
                                                        g.colorsLength = 6;
                                                        g.alphasLength = 2;
                                                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                        g.colors[5] = float4(1, 1, 1, 1);
                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                        g.alphas[0] = float2(1, 0);
                                                        g.alphas[1] = float2(1, 1);
                                                        g.alphas[2] = float2(0, 0);
                                                        g.alphas[3] = float2(0, 0);
                                                        g.alphas[4] = float2(0, 0);
                                                        g.alphas[5] = float2(0, 0);
                                                        g.alphas[6] = float2(0, 0);
                                                        g.alphas[7] = float2(0, 0);
                                                        return g;
                                                    }
                                                    #define _GradientStandard _GradientStandard_Definition()
                                                    Gradient _GradientRedStrength_Definition()
                                                    {
                                                        Gradient g;
                                                        g.type = 0;
                                                        g.colorsLength = 5;
                                                        g.alphasLength = 2;
                                                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                        g.colors[4] = float4(1, 0, 0, 1);
                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                        g.alphas[0] = float2(1, 0);
                                                        g.alphas[1] = float2(1, 1);
                                                        g.alphas[2] = float2(0, 0);
                                                        g.alphas[3] = float2(0, 0);
                                                        g.alphas[4] = float2(0, 0);
                                                        g.alphas[5] = float2(0, 0);
                                                        g.alphas[6] = float2(0, 0);
                                                        g.alphas[7] = float2(0, 0);
                                                        return g;
                                                    }
                                                    #define _GradientRedStrength _GradientRedStrength_Definition()
                                                    Gradient _GradientWhiteHot_Definition()
                                                    {
                                                        Gradient g;
                                                        g.type = 0;
                                                        g.colorsLength = 3;
                                                        g.alphasLength = 2;
                                                        g.colors[0] = float4(0, 0, 0, 0);
                                                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                        g.colors[2] = float4(1, 1, 1, 1);
                                                        g.colors[3] = float4(0, 0, 0, 0);
                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                        g.alphas[0] = float2(1, 0);
                                                        g.alphas[1] = float2(1, 1);
                                                        g.alphas[2] = float2(0, 0);
                                                        g.alphas[3] = float2(0, 0);
                                                        g.alphas[4] = float2(0, 0);
                                                        g.alphas[5] = float2(0, 0);
                                                        g.alphas[6] = float2(0, 0);
                                                        g.alphas[7] = float2(0, 0);
                                                        return g;
                                                    }
                                                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                    Gradient _GradientBlackHot_Definition()
                                                    {
                                                        Gradient g;
                                                        g.type = 0;
                                                        g.colorsLength = 4;
                                                        g.alphasLength = 2;
                                                        g.colors[0] = float4(1, 1, 1, 0);
                                                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                        g.colors[3] = float4(0, 0, 0, 1);
                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                        g.alphas[0] = float2(1, 0);
                                                        g.alphas[1] = float2(1, 1);
                                                        g.alphas[2] = float2(0, 0);
                                                        g.alphas[3] = float2(0, 0);
                                                        g.alphas[4] = float2(0, 0);
                                                        g.alphas[5] = float2(0, 0);
                                                        g.alphas[6] = float2(0, 0);
                                                        g.alphas[7] = float2(0, 0);
                                                        return g;
                                                    }
                                                    #define _GradientBlackHot _GradientBlackHot_Definition()

                                                    // Graph Includes
                                                    // GraphIncludes: <None>

                                                    // -- Property used by ScenePickingPass
                                                    #ifdef SCENEPICKINGPASS
                                                    float4 _SelectionID;
                                                    #endif

                                                    // -- Properties used by SceneSelectionPass
                                                    #ifdef SCENESELECTIONPASS
                                                    int _ObjectId;
                                                    int _PassValue;
                                                    #endif

                                                    // Graph Functions

                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                    {
                                                        Out = A / B;
                                                    }

                                                    // Custom interpolators pre vertex
                                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                    // Graph Vertex
                                                    struct VertexDescription
                                                    {
                                                        float3 Position;
                                                        float3 Normal;
                                                        float3 Tangent;
                                                    };

                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                    {
                                                        VertexDescription description = (VertexDescription)0;
                                                        description.Position = IN.ObjectSpacePosition;
                                                        description.Normal = IN.ObjectSpaceNormal;
                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                        return description;
                                                    }

                                                    // Custom interpolators, pre surface
                                                    #ifdef FEATURES_GRAPH_VERTEX
                                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                    {
                                                    return output;
                                                    }
                                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                    #endif

                                                    // Graph Pixel
                                                    struct SurfaceDescription
                                                    {
                                                        float Alpha;
                                                    };

                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                    {
                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                        return surface;
                                                    }

                                                    // --------------------------------------------------
                                                    // Build Graph Inputs
                                                    #ifdef HAVE_VFX_MODIFICATION
                                                    #define VFX_SRP_ATTRIBUTES Attributes
                                                    #define VFX_SRP_VARYINGS Varyings
                                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                    #endif
                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                    {
                                                        VertexDescriptionInputs output;
                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                        output.ObjectSpaceNormal = input.normalOS;
                                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                        output.ObjectSpacePosition = input.positionOS;

                                                        return output;
                                                    }
                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                    {
                                                        SurfaceDescriptionInputs output;
                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                    #ifdef HAVE_VFX_MODIFICATION
                                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                    #endif







                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                    #else
                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                    #endif
                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                            return output;
                                                    }

                                                    // --------------------------------------------------
                                                    // Main

                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                                                    // --------------------------------------------------
                                                    // Visual Effect Vertex Invocations
                                                    #ifdef HAVE_VFX_MODIFICATION
                                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                    #endif

                                                    ENDHLSL
                                                    }
    }
        SubShader
                                                    {
                                                        Tags
                                                        {
                                                            "RenderPipeline" = "UniversalPipeline"
                                                            "RenderType" = "Transparent"
                                                            "UniversalMaterialType" = "Unlit"
                                                            "Queue" = "Transparent"
                                                            "ShaderGraphShader" = "true"
                                                            "ShaderGraphTargetId" = "UniversalUnlitSubTarget"
                                                        }
                                                        Pass
                                                        {
                                                            Name "Universal Forward"
                                                            Tags
                                                            {
                                                            // LightMode: <None>
                                                        }

                                                        // Render State
                                                        Cull Back
                                                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                                                        ZTest Always
                                                        ZWrite Off

                                                        // Debug
                                                        // <None>

                                                        // --------------------------------------------------
                                                        // Pass

                                                        HLSLPROGRAM

                                                        // Pragmas
                                                        #pragma target 2.0
                                                        #pragma only_renderers gles gles3 glcore d3d11
                                                        #pragma multi_compile_instancing
                                                        #pragma multi_compile_fog
                                                        #pragma instancing_options renderinglayer
                                                        #pragma vertex vert
                                                        #pragma fragment frag

                                                        // DotsInstancingOptions: <None>
                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                        // Keywords
                                                        #pragma multi_compile _ LIGHTMAP_ON
                                                        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                                                        #pragma shader_feature _ _SAMPLE_GI
                                                        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
                                                        #pragma multi_compile_fragment _ DEBUG_DISPLAY
                                                        // GraphKeywords: <None>

                                                        // Defines

                                                        #define ATTRIBUTES_NEED_NORMAL
                                                        #define ATTRIBUTES_NEED_TANGENT
                                                        #define VARYINGS_NEED_POSITION_WS
                                                        #define VARYINGS_NEED_NORMAL_WS
                                                        #define VARYINGS_NEED_VIEWDIRECTION_WS
                                                        #define FEATURES_GRAPH_VERTEX
                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                        #define SHADERPASS SHADERPASS_UNLIT
                                                        #define _FOG_FRAGMENT 1
                                                        #define _SURFACE_TYPE_TRANSPARENT 1
                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                        // custom interpolator pre-include
                                                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                        // Includes
                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
                                                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                        // --------------------------------------------------
                                                        // Structs and Packing

                                                        // custom interpolators pre packing
                                                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                        struct Attributes
                                                        {
                                                             float3 positionOS : POSITION;
                                                             float3 normalOS : NORMAL;
                                                             float4 tangentOS : TANGENT;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                             uint instanceID : INSTANCEID_SEMANTIC;
                                                            #endif
                                                        };
                                                        struct Varyings
                                                        {
                                                             float4 positionCS : SV_POSITION;
                                                             float3 positionWS;
                                                             float3 normalWS;
                                                             float3 viewDirectionWS;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                            #endif
                                                        };
                                                        struct SurfaceDescriptionInputs
                                                        {
                                                             float3 WorldSpaceNormal;
                                                             float3 WorldSpaceViewDirection;
                                                        };
                                                        struct VertexDescriptionInputs
                                                        {
                                                             float3 ObjectSpaceNormal;
                                                             float3 ObjectSpaceTangent;
                                                             float3 ObjectSpacePosition;
                                                        };
                                                        struct PackedVaryings
                                                        {
                                                             float4 positionCS : SV_POSITION;
                                                             float3 interp0 : INTERP0;
                                                             float3 interp1 : INTERP1;
                                                             float3 interp2 : INTERP2;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                            #endif
                                                        };

                                                        PackedVaryings PackVaryings(Varyings input)
                                                        {
                                                            PackedVaryings output;
                                                            ZERO_INITIALIZE(PackedVaryings, output);
                                                            output.positionCS = input.positionCS;
                                                            output.interp0.xyz = input.positionWS;
                                                            output.interp1.xyz = input.normalWS;
                                                            output.interp2.xyz = input.viewDirectionWS;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            output.instanceID = input.instanceID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            output.cullFace = input.cullFace;
                                                            #endif
                                                            return output;
                                                        }

                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                        {
                                                            Varyings output;
                                                            output.positionCS = input.positionCS;
                                                            output.positionWS = input.interp0.xyz;
                                                            output.normalWS = input.interp1.xyz;
                                                            output.viewDirectionWS = input.interp2.xyz;
                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                            output.instanceID = input.instanceID;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                            #endif
                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                            #endif
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            output.cullFace = input.cullFace;
                                                            #endif
                                                            return output;
                                                        }


                                                        // --------------------------------------------------
                                                        // Graph

                                                        // Graph Properties
                                                        CBUFFER_START(UnityPerMaterial)
                                                        float _Temperature;
                                                        CBUFFER_END

                                                            // Object and Global properties
                                                            Gradient _GradientStandard_Definition()
                                                            {
                                                                Gradient g;
                                                                g.type = 0;
                                                                g.colorsLength = 6;
                                                                g.alphasLength = 2;
                                                                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                g.colors[5] = float4(1, 1, 1, 1);
                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                g.alphas[0] = float2(1, 0);
                                                                g.alphas[1] = float2(1, 1);
                                                                g.alphas[2] = float2(0, 0);
                                                                g.alphas[3] = float2(0, 0);
                                                                g.alphas[4] = float2(0, 0);
                                                                g.alphas[5] = float2(0, 0);
                                                                g.alphas[6] = float2(0, 0);
                                                                g.alphas[7] = float2(0, 0);
                                                                return g;
                                                            }
                                                            #define _GradientStandard _GradientStandard_Definition()
                                                            Gradient _GradientRedStrength_Definition()
                                                            {
                                                                Gradient g;
                                                                g.type = 0;
                                                                g.colorsLength = 5;
                                                                g.alphasLength = 2;
                                                                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                g.colors[4] = float4(1, 0, 0, 1);
                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                g.alphas[0] = float2(1, 0);
                                                                g.alphas[1] = float2(1, 1);
                                                                g.alphas[2] = float2(0, 0);
                                                                g.alphas[3] = float2(0, 0);
                                                                g.alphas[4] = float2(0, 0);
                                                                g.alphas[5] = float2(0, 0);
                                                                g.alphas[6] = float2(0, 0);
                                                                g.alphas[7] = float2(0, 0);
                                                                return g;
                                                            }
                                                            #define _GradientRedStrength _GradientRedStrength_Definition()
                                                            Gradient _GradientWhiteHot_Definition()
                                                            {
                                                                Gradient g;
                                                                g.type = 0;
                                                                g.colorsLength = 3;
                                                                g.alphasLength = 2;
                                                                g.colors[0] = float4(0, 0, 0, 0);
                                                                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                g.colors[2] = float4(1, 1, 1, 1);
                                                                g.colors[3] = float4(0, 0, 0, 0);
                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                g.alphas[0] = float2(1, 0);
                                                                g.alphas[1] = float2(1, 1);
                                                                g.alphas[2] = float2(0, 0);
                                                                g.alphas[3] = float2(0, 0);
                                                                g.alphas[4] = float2(0, 0);
                                                                g.alphas[5] = float2(0, 0);
                                                                g.alphas[6] = float2(0, 0);
                                                                g.alphas[7] = float2(0, 0);
                                                                return g;
                                                            }
                                                            #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                            Gradient _GradientBlackHot_Definition()
                                                            {
                                                                Gradient g;
                                                                g.type = 0;
                                                                g.colorsLength = 4;
                                                                g.alphasLength = 2;
                                                                g.colors[0] = float4(1, 1, 1, 0);
                                                                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                g.colors[3] = float4(0, 0, 0, 1);
                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                g.alphas[0] = float2(1, 0);
                                                                g.alphas[1] = float2(1, 1);
                                                                g.alphas[2] = float2(0, 0);
                                                                g.alphas[3] = float2(0, 0);
                                                                g.alphas[4] = float2(0, 0);
                                                                g.alphas[5] = float2(0, 0);
                                                                g.alphas[6] = float2(0, 0);
                                                                g.alphas[7] = float2(0, 0);
                                                                return g;
                                                            }
                                                            #define _GradientBlackHot _GradientBlackHot_Definition()

                                                            // Graph Includes
                                                            // GraphIncludes: <None>

                                                            // -- Property used by ScenePickingPass
                                                            #ifdef SCENEPICKINGPASS
                                                            float4 _SelectionID;
                                                            #endif

                                                            // -- Properties used by SceneSelectionPass
                                                            #ifdef SCENESELECTIONPASS
                                                            int _ObjectId;
                                                            int _PassValue;
                                                            #endif

                                                            // Graph Functions

                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                            {
                                                                Out = A / B;
                                                            }

                                                            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
                                                            {
                                                                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
                                                            }

                                                            void Unity_InvertColors_float(float In, float InvertColors, out float Out)
                                                            {
                                                                Out = abs(InvertColors - In);
                                                            }

                                                            void Unity_Multiply_float_float(float A, float B, out float Out)
                                                            {
                                                                Out = A * B;
                                                            }

                                                            void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
                                                            {
                                                                float3 color = Gradient.colors[0].rgb;
                                                                [unroll]
                                                                for (int c = 1; c < Gradient.colorsLength; c++)
                                                                {
                                                                    float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                                                                    color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
                                                                }
                                                            #ifdef UNITY_COLORSPACE_GAMMA
                                                                color = LinearToSRGB(color);
                                                            #endif
                                                                float alpha = Gradient.alphas[0].x;
                                                                [unroll]
                                                                for (int a = 1; a < Gradient.alphasLength; a++)
                                                                {
                                                                    float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                                                                    alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
                                                                }
                                                                Out = float4(color, alpha);
                                                            }

                                                            // Custom interpolators pre vertex
                                                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                            // Graph Vertex
                                                            struct VertexDescription
                                                            {
                                                                float3 Position;
                                                                float3 Normal;
                                                                float3 Tangent;
                                                            };

                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                            {
                                                                VertexDescription description = (VertexDescription)0;
                                                                description.Position = IN.ObjectSpacePosition;
                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                return description;
                                                            }

                                                            // Custom interpolators, pre surface
                                                            #ifdef FEATURES_GRAPH_VERTEX
                                                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                            {
                                                            return output;
                                                            }
                                                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                            #endif

                                                            // Graph Pixel
                                                            struct SurfaceDescription
                                                            {
                                                                float3 BaseColor;
                                                                float Alpha;
                                                            };

                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                            {
                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                Gradient _Property_ca36ee881dbd41e18ba6d0819a5219b2_Out_0 = _GradientWhiteHot;
                                                                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                float _FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3;
                                                                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, -0.4, _FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3);
                                                                float _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1;
                                                                float _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_InvertColors = float(1);
                                                                Unity_InvertColors_float(_FresnelEffect_2ab7e45080af4b13b887a8b4adb27fcb_Out_3, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_InvertColors, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1);
                                                                float _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2;
                                                                Unity_Multiply_float_float(_Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2, _InvertColors_1dfdee7e341b4ee0afc1a6786dbd3bf3_Out_1, _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2);
                                                                float4 _SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2;
                                                                Unity_SampleGradientV1_float(_Property_ca36ee881dbd41e18ba6d0819a5219b2_Out_0, _Multiply_5caac94c233a4e2e9905b91aa4048fb5_Out_2, _SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2);
                                                                surface.BaseColor = (_SampleGradient_9630b58f660448da9b03f0aa70f4f130_Out_2.xyz);
                                                                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                return surface;
                                                            }

                                                            // --------------------------------------------------
                                                            // Build Graph Inputs
                                                            #ifdef HAVE_VFX_MODIFICATION
                                                            #define VFX_SRP_ATTRIBUTES Attributes
                                                            #define VFX_SRP_VARYINGS Varyings
                                                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                            #endif
                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                            {
                                                                VertexDescriptionInputs output;
                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                output.ObjectSpacePosition = input.positionOS;

                                                                return output;
                                                            }
                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                            {
                                                                SurfaceDescriptionInputs output;
                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                            #endif



                                                                // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
                                                                float3 unnormalizedNormalWS = input.normalWS;
                                                                const float renormFactor = 1.0 / length(unnormalizedNormalWS);


                                                                output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph


                                                                output.WorldSpaceViewDirection = normalize(input.viewDirectionWS);
                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                            #else
                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                            #endif
                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                    return output;
                                                            }

                                                            // --------------------------------------------------
                                                            // Main

                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

                                                            // --------------------------------------------------
                                                            // Visual Effect Vertex Invocations
                                                            #ifdef HAVE_VFX_MODIFICATION
                                                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                            #endif

                                                            ENDHLSL
                                                            }
                                                            Pass
                                                            {
                                                                Name "DepthNormalsOnly"
                                                                Tags
                                                                {
                                                                    "LightMode" = "DepthNormalsOnly"
                                                                }

                                                                // Render State
                                                                Cull Back
                                                                ZTest Always
                                                                ZWrite On

                                                                // Debug
                                                                // <None>

                                                                // --------------------------------------------------
                                                                // Pass

                                                                HLSLPROGRAM

                                                                // Pragmas
                                                                #pragma target 2.0
                                                                #pragma only_renderers gles gles3 glcore d3d11
                                                                #pragma multi_compile_instancing
                                                                #pragma vertex vert
                                                                #pragma fragment frag

                                                                // DotsInstancingOptions: <None>
                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                // Keywords
                                                                // PassKeywords: <None>
                                                                // GraphKeywords: <None>

                                                                // Defines

                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                #define ATTRIBUTES_NEED_TEXCOORD1
                                                                #define VARYINGS_NEED_NORMAL_WS
                                                                #define VARYINGS_NEED_TANGENT_WS
                                                                #define FEATURES_GRAPH_VERTEX
                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                                // custom interpolator pre-include
                                                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                                // Includes
                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                                // --------------------------------------------------
                                                                // Structs and Packing

                                                                // custom interpolators pre packing
                                                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                                struct Attributes
                                                                {
                                                                     float3 positionOS : POSITION;
                                                                     float3 normalOS : NORMAL;
                                                                     float4 tangentOS : TANGENT;
                                                                     float4 uv1 : TEXCOORD1;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                     uint instanceID : INSTANCEID_SEMANTIC;
                                                                    #endif
                                                                };
                                                                struct Varyings
                                                                {
                                                                     float4 positionCS : SV_POSITION;
                                                                     float3 normalWS;
                                                                     float4 tangentWS;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                    #endif
                                                                };
                                                                struct SurfaceDescriptionInputs
                                                                {
                                                                };
                                                                struct VertexDescriptionInputs
                                                                {
                                                                     float3 ObjectSpaceNormal;
                                                                     float3 ObjectSpaceTangent;
                                                                     float3 ObjectSpacePosition;
                                                                };
                                                                struct PackedVaryings
                                                                {
                                                                     float4 positionCS : SV_POSITION;
                                                                     float3 interp0 : INTERP0;
                                                                     float4 interp1 : INTERP1;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                    #endif
                                                                };

                                                                PackedVaryings PackVaryings(Varyings input)
                                                                {
                                                                    PackedVaryings output;
                                                                    ZERO_INITIALIZE(PackedVaryings, output);
                                                                    output.positionCS = input.positionCS;
                                                                    output.interp0.xyz = input.normalWS;
                                                                    output.interp1.xyzw = input.tangentWS;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    output.instanceID = input.instanceID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    output.cullFace = input.cullFace;
                                                                    #endif
                                                                    return output;
                                                                }

                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                {
                                                                    Varyings output;
                                                                    output.positionCS = input.positionCS;
                                                                    output.normalWS = input.interp0.xyz;
                                                                    output.tangentWS = input.interp1.xyzw;
                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                    output.instanceID = input.instanceID;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                    #endif
                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                    #endif
                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    output.cullFace = input.cullFace;
                                                                    #endif
                                                                    return output;
                                                                }


                                                                // --------------------------------------------------
                                                                // Graph

                                                                // Graph Properties
                                                                CBUFFER_START(UnityPerMaterial)
                                                                float _Temperature;
                                                                CBUFFER_END

                                                                    // Object and Global properties
                                                                    Gradient _GradientStandard_Definition()
                                                                    {
                                                                        Gradient g;
                                                                        g.type = 0;
                                                                        g.colorsLength = 6;
                                                                        g.alphasLength = 2;
                                                                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                        g.colors[5] = float4(1, 1, 1, 1);
                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                        g.alphas[0] = float2(1, 0);
                                                                        g.alphas[1] = float2(1, 1);
                                                                        g.alphas[2] = float2(0, 0);
                                                                        g.alphas[3] = float2(0, 0);
                                                                        g.alphas[4] = float2(0, 0);
                                                                        g.alphas[5] = float2(0, 0);
                                                                        g.alphas[6] = float2(0, 0);
                                                                        g.alphas[7] = float2(0, 0);
                                                                        return g;
                                                                    }
                                                                    #define _GradientStandard _GradientStandard_Definition()
                                                                    Gradient _GradientRedStrength_Definition()
                                                                    {
                                                                        Gradient g;
                                                                        g.type = 0;
                                                                        g.colorsLength = 5;
                                                                        g.alphasLength = 2;
                                                                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                        g.colors[4] = float4(1, 0, 0, 1);
                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                        g.alphas[0] = float2(1, 0);
                                                                        g.alphas[1] = float2(1, 1);
                                                                        g.alphas[2] = float2(0, 0);
                                                                        g.alphas[3] = float2(0, 0);
                                                                        g.alphas[4] = float2(0, 0);
                                                                        g.alphas[5] = float2(0, 0);
                                                                        g.alphas[6] = float2(0, 0);
                                                                        g.alphas[7] = float2(0, 0);
                                                                        return g;
                                                                    }
                                                                    #define _GradientRedStrength _GradientRedStrength_Definition()
                                                                    Gradient _GradientWhiteHot_Definition()
                                                                    {
                                                                        Gradient g;
                                                                        g.type = 0;
                                                                        g.colorsLength = 3;
                                                                        g.alphasLength = 2;
                                                                        g.colors[0] = float4(0, 0, 0, 0);
                                                                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                        g.colors[2] = float4(1, 1, 1, 1);
                                                                        g.colors[3] = float4(0, 0, 0, 0);
                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                        g.alphas[0] = float2(1, 0);
                                                                        g.alphas[1] = float2(1, 1);
                                                                        g.alphas[2] = float2(0, 0);
                                                                        g.alphas[3] = float2(0, 0);
                                                                        g.alphas[4] = float2(0, 0);
                                                                        g.alphas[5] = float2(0, 0);
                                                                        g.alphas[6] = float2(0, 0);
                                                                        g.alphas[7] = float2(0, 0);
                                                                        return g;
                                                                    }
                                                                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                                    Gradient _GradientBlackHot_Definition()
                                                                    {
                                                                        Gradient g;
                                                                        g.type = 0;
                                                                        g.colorsLength = 4;
                                                                        g.alphasLength = 2;
                                                                        g.colors[0] = float4(1, 1, 1, 0);
                                                                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                        g.colors[3] = float4(0, 0, 0, 1);
                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                        g.alphas[0] = float2(1, 0);
                                                                        g.alphas[1] = float2(1, 1);
                                                                        g.alphas[2] = float2(0, 0);
                                                                        g.alphas[3] = float2(0, 0);
                                                                        g.alphas[4] = float2(0, 0);
                                                                        g.alphas[5] = float2(0, 0);
                                                                        g.alphas[6] = float2(0, 0);
                                                                        g.alphas[7] = float2(0, 0);
                                                                        return g;
                                                                    }
                                                                    #define _GradientBlackHot _GradientBlackHot_Definition()

                                                                    // Graph Includes
                                                                    // GraphIncludes: <None>

                                                                    // -- Property used by ScenePickingPass
                                                                    #ifdef SCENEPICKINGPASS
                                                                    float4 _SelectionID;
                                                                    #endif

                                                                    // -- Properties used by SceneSelectionPass
                                                                    #ifdef SCENESELECTIONPASS
                                                                    int _ObjectId;
                                                                    int _PassValue;
                                                                    #endif

                                                                    // Graph Functions

                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                    {
                                                                        Out = A / B;
                                                                    }

                                                                    // Custom interpolators pre vertex
                                                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                                    // Graph Vertex
                                                                    struct VertexDescription
                                                                    {
                                                                        float3 Position;
                                                                        float3 Normal;
                                                                        float3 Tangent;
                                                                    };

                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                    {
                                                                        VertexDescription description = (VertexDescription)0;
                                                                        description.Position = IN.ObjectSpacePosition;
                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                        return description;
                                                                    }

                                                                    // Custom interpolators, pre surface
                                                                    #ifdef FEATURES_GRAPH_VERTEX
                                                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                                    {
                                                                    return output;
                                                                    }
                                                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                                    #endif

                                                                    // Graph Pixel
                                                                    struct SurfaceDescription
                                                                    {
                                                                        float Alpha;
                                                                    };

                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                    {
                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                        return surface;
                                                                    }

                                                                    // --------------------------------------------------
                                                                    // Build Graph Inputs
                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                    #define VFX_SRP_ATTRIBUTES Attributes
                                                                    #define VFX_SRP_VARYINGS Varyings
                                                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                                    #endif
                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                    {
                                                                        VertexDescriptionInputs output;
                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                        return output;
                                                                    }
                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                    {
                                                                        SurfaceDescriptionInputs output;
                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                                    #endif







                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                    #else
                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                    #endif
                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                            return output;
                                                                    }

                                                                    // --------------------------------------------------
                                                                    // Main

                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                                                                    // --------------------------------------------------
                                                                    // Visual Effect Vertex Invocations
                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                                    #endif

                                                                    ENDHLSL
                                                                    }
                                                                    Pass
                                                                    {
                                                                        Name "ShadowCaster"
                                                                        Tags
                                                                        {
                                                                            "LightMode" = "ShadowCaster"
                                                                        }

                                                                        // Render State
                                                                        Cull Back
                                                                        ZTest Always
                                                                        ZWrite On
                                                                        ColorMask 0

                                                                        // Debug
                                                                        // <None>

                                                                        // --------------------------------------------------
                                                                        // Pass

                                                                        HLSLPROGRAM

                                                                        // Pragmas
                                                                        #pragma target 2.0
                                                                        #pragma only_renderers gles gles3 glcore d3d11
                                                                        #pragma multi_compile_instancing
                                                                        #pragma vertex vert
                                                                        #pragma fragment frag

                                                                        // DotsInstancingOptions: <None>
                                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                                        // Keywords
                                                                        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
                                                                        // GraphKeywords: <None>

                                                                        // Defines

                                                                        #define ATTRIBUTES_NEED_NORMAL
                                                                        #define ATTRIBUTES_NEED_TANGENT
                                                                        #define VARYINGS_NEED_NORMAL_WS
                                                                        #define FEATURES_GRAPH_VERTEX
                                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                        #define SHADERPASS SHADERPASS_SHADOWCASTER
                                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                                        // custom interpolator pre-include
                                                                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                                        // Includes
                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                                        // --------------------------------------------------
                                                                        // Structs and Packing

                                                                        // custom interpolators pre packing
                                                                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                                        struct Attributes
                                                                        {
                                                                             float3 positionOS : POSITION;
                                                                             float3 normalOS : NORMAL;
                                                                             float4 tangentOS : TANGENT;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                             uint instanceID : INSTANCEID_SEMANTIC;
                                                                            #endif
                                                                        };
                                                                        struct Varyings
                                                                        {
                                                                             float4 positionCS : SV_POSITION;
                                                                             float3 normalWS;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                            #endif
                                                                        };
                                                                        struct SurfaceDescriptionInputs
                                                                        {
                                                                        };
                                                                        struct VertexDescriptionInputs
                                                                        {
                                                                             float3 ObjectSpaceNormal;
                                                                             float3 ObjectSpaceTangent;
                                                                             float3 ObjectSpacePosition;
                                                                        };
                                                                        struct PackedVaryings
                                                                        {
                                                                             float4 positionCS : SV_POSITION;
                                                                             float3 interp0 : INTERP0;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                            #endif
                                                                        };

                                                                        PackedVaryings PackVaryings(Varyings input)
                                                                        {
                                                                            PackedVaryings output;
                                                                            ZERO_INITIALIZE(PackedVaryings, output);
                                                                            output.positionCS = input.positionCS;
                                                                            output.interp0.xyz = input.normalWS;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            output.instanceID = input.instanceID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            output.cullFace = input.cullFace;
                                                                            #endif
                                                                            return output;
                                                                        }

                                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                                        {
                                                                            Varyings output;
                                                                            output.positionCS = input.positionCS;
                                                                            output.normalWS = input.interp0.xyz;
                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                            output.instanceID = input.instanceID;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                            #endif
                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                            #endif
                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            output.cullFace = input.cullFace;
                                                                            #endif
                                                                            return output;
                                                                        }


                                                                        // --------------------------------------------------
                                                                        // Graph

                                                                        // Graph Properties
                                                                        CBUFFER_START(UnityPerMaterial)
                                                                        float _Temperature;
                                                                        CBUFFER_END

                                                                            // Object and Global properties
                                                                            Gradient _GradientStandard_Definition()
                                                                            {
                                                                                Gradient g;
                                                                                g.type = 0;
                                                                                g.colorsLength = 6;
                                                                                g.alphasLength = 2;
                                                                                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                                g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                                g.colors[5] = float4(1, 1, 1, 1);
                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                g.alphas[0] = float2(1, 0);
                                                                                g.alphas[1] = float2(1, 1);
                                                                                g.alphas[2] = float2(0, 0);
                                                                                g.alphas[3] = float2(0, 0);
                                                                                g.alphas[4] = float2(0, 0);
                                                                                g.alphas[5] = float2(0, 0);
                                                                                g.alphas[6] = float2(0, 0);
                                                                                g.alphas[7] = float2(0, 0);
                                                                                return g;
                                                                            }
                                                                            #define _GradientStandard _GradientStandard_Definition()
                                                                            Gradient _GradientRedStrength_Definition()
                                                                            {
                                                                                Gradient g;
                                                                                g.type = 0;
                                                                                g.colorsLength = 5;
                                                                                g.alphasLength = 2;
                                                                                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                                g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                                g.colors[4] = float4(1, 0, 0, 1);
                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                g.alphas[0] = float2(1, 0);
                                                                                g.alphas[1] = float2(1, 1);
                                                                                g.alphas[2] = float2(0, 0);
                                                                                g.alphas[3] = float2(0, 0);
                                                                                g.alphas[4] = float2(0, 0);
                                                                                g.alphas[5] = float2(0, 0);
                                                                                g.alphas[6] = float2(0, 0);
                                                                                g.alphas[7] = float2(0, 0);
                                                                                return g;
                                                                            }
                                                                            #define _GradientRedStrength _GradientRedStrength_Definition()
                                                                            Gradient _GradientWhiteHot_Definition()
                                                                            {
                                                                                Gradient g;
                                                                                g.type = 0;
                                                                                g.colorsLength = 3;
                                                                                g.alphasLength = 2;
                                                                                g.colors[0] = float4(0, 0, 0, 0);
                                                                                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                                g.colors[2] = float4(1, 1, 1, 1);
                                                                                g.colors[3] = float4(0, 0, 0, 0);
                                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                g.alphas[0] = float2(1, 0);
                                                                                g.alphas[1] = float2(1, 1);
                                                                                g.alphas[2] = float2(0, 0);
                                                                                g.alphas[3] = float2(0, 0);
                                                                                g.alphas[4] = float2(0, 0);
                                                                                g.alphas[5] = float2(0, 0);
                                                                                g.alphas[6] = float2(0, 0);
                                                                                g.alphas[7] = float2(0, 0);
                                                                                return g;
                                                                            }
                                                                            #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                                            Gradient _GradientBlackHot_Definition()
                                                                            {
                                                                                Gradient g;
                                                                                g.type = 0;
                                                                                g.colorsLength = 4;
                                                                                g.alphasLength = 2;
                                                                                g.colors[0] = float4(1, 1, 1, 0);
                                                                                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                                g.colors[3] = float4(0, 0, 0, 1);
                                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                g.alphas[0] = float2(1, 0);
                                                                                g.alphas[1] = float2(1, 1);
                                                                                g.alphas[2] = float2(0, 0);
                                                                                g.alphas[3] = float2(0, 0);
                                                                                g.alphas[4] = float2(0, 0);
                                                                                g.alphas[5] = float2(0, 0);
                                                                                g.alphas[6] = float2(0, 0);
                                                                                g.alphas[7] = float2(0, 0);
                                                                                return g;
                                                                            }
                                                                            #define _GradientBlackHot _GradientBlackHot_Definition()

                                                                            // Graph Includes
                                                                            // GraphIncludes: <None>

                                                                            // -- Property used by ScenePickingPass
                                                                            #ifdef SCENEPICKINGPASS
                                                                            float4 _SelectionID;
                                                                            #endif

                                                                            // -- Properties used by SceneSelectionPass
                                                                            #ifdef SCENESELECTIONPASS
                                                                            int _ObjectId;
                                                                            int _PassValue;
                                                                            #endif

                                                                            // Graph Functions

                                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                                            {
                                                                                Out = A / B;
                                                                            }

                                                                            // Custom interpolators pre vertex
                                                                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                                            // Graph Vertex
                                                                            struct VertexDescription
                                                                            {
                                                                                float3 Position;
                                                                                float3 Normal;
                                                                                float3 Tangent;
                                                                            };

                                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                            {
                                                                                VertexDescription description = (VertexDescription)0;
                                                                                description.Position = IN.ObjectSpacePosition;
                                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                                return description;
                                                                            }

                                                                            // Custom interpolators, pre surface
                                                                            #ifdef FEATURES_GRAPH_VERTEX
                                                                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                                            {
                                                                            return output;
                                                                            }
                                                                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                                            #endif

                                                                            // Graph Pixel
                                                                            struct SurfaceDescription
                                                                            {
                                                                                float Alpha;
                                                                            };

                                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                            {
                                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                return surface;
                                                                            }

                                                                            // --------------------------------------------------
                                                                            // Build Graph Inputs
                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                            #define VFX_SRP_ATTRIBUTES Attributes
                                                                            #define VFX_SRP_VARYINGS Varyings
                                                                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                                            #endif
                                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                            {
                                                                                VertexDescriptionInputs output;
                                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                                output.ObjectSpacePosition = input.positionOS;

                                                                                return output;
                                                                            }
                                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                            {
                                                                                SurfaceDescriptionInputs output;
                                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                                            #endif







                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                            #else
                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                            #endif
                                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                    return output;
                                                                            }

                                                                            // --------------------------------------------------
                                                                            // Main

                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                                                                            // --------------------------------------------------
                                                                            // Visual Effect Vertex Invocations
                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                                            #endif

                                                                            ENDHLSL
                                                                            }
                                                                            Pass
                                                                            {
                                                                                Name "SceneSelectionPass"
                                                                                Tags
                                                                                {
                                                                                    "LightMode" = "SceneSelectionPass"
                                                                                }

                                                                                // Render State
                                                                                Cull Off

                                                                                // Debug
                                                                                // <None>

                                                                                // --------------------------------------------------
                                                                                // Pass

                                                                                HLSLPROGRAM

                                                                                // Pragmas
                                                                                #pragma target 2.0
                                                                                #pragma only_renderers gles gles3 glcore d3d11
                                                                                #pragma multi_compile_instancing
                                                                                #pragma vertex vert
                                                                                #pragma fragment frag

                                                                                // DotsInstancingOptions: <None>
                                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                                // Keywords
                                                                                // PassKeywords: <None>
                                                                                // GraphKeywords: <None>

                                                                                // Defines

                                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                                #define FEATURES_GRAPH_VERTEX
                                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                #define SHADERPASS SHADERPASS_DEPTHONLY
                                                                                #define SCENESELECTIONPASS 1
                                                                                #define ALPHA_CLIP_THRESHOLD 1
                                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                                                // custom interpolator pre-include
                                                                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                                                // Includes
                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                                                // --------------------------------------------------
                                                                                // Structs and Packing

                                                                                // custom interpolators pre packing
                                                                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                                                struct Attributes
                                                                                {
                                                                                     float3 positionOS : POSITION;
                                                                                     float3 normalOS : NORMAL;
                                                                                     float4 tangentOS : TANGENT;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                     uint instanceID : INSTANCEID_SEMANTIC;
                                                                                    #endif
                                                                                };
                                                                                struct Varyings
                                                                                {
                                                                                     float4 positionCS : SV_POSITION;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                    #endif
                                                                                };
                                                                                struct SurfaceDescriptionInputs
                                                                                {
                                                                                };
                                                                                struct VertexDescriptionInputs
                                                                                {
                                                                                     float3 ObjectSpaceNormal;
                                                                                     float3 ObjectSpaceTangent;
                                                                                     float3 ObjectSpacePosition;
                                                                                };
                                                                                struct PackedVaryings
                                                                                {
                                                                                     float4 positionCS : SV_POSITION;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                    #endif
                                                                                };

                                                                                PackedVaryings PackVaryings(Varyings input)
                                                                                {
                                                                                    PackedVaryings output;
                                                                                    ZERO_INITIALIZE(PackedVaryings, output);
                                                                                    output.positionCS = input.positionCS;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    output.instanceID = input.instanceID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    output.cullFace = input.cullFace;
                                                                                    #endif
                                                                                    return output;
                                                                                }

                                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                                {
                                                                                    Varyings output;
                                                                                    output.positionCS = input.positionCS;
                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                    output.instanceID = input.instanceID;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                    #endif
                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                    #endif
                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    output.cullFace = input.cullFace;
                                                                                    #endif
                                                                                    return output;
                                                                                }


                                                                                // --------------------------------------------------
                                                                                // Graph

                                                                                // Graph Properties
                                                                                CBUFFER_START(UnityPerMaterial)
                                                                                float _Temperature;
                                                                                CBUFFER_END

                                                                                    // Object and Global properties
                                                                                    Gradient _GradientStandard_Definition()
                                                                                    {
                                                                                        Gradient g;
                                                                                        g.type = 0;
                                                                                        g.colorsLength = 6;
                                                                                        g.alphasLength = 2;
                                                                                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                                        g.colors[5] = float4(1, 1, 1, 1);
                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                        g.alphas[0] = float2(1, 0);
                                                                                        g.alphas[1] = float2(1, 1);
                                                                                        g.alphas[2] = float2(0, 0);
                                                                                        g.alphas[3] = float2(0, 0);
                                                                                        g.alphas[4] = float2(0, 0);
                                                                                        g.alphas[5] = float2(0, 0);
                                                                                        g.alphas[6] = float2(0, 0);
                                                                                        g.alphas[7] = float2(0, 0);
                                                                                        return g;
                                                                                    }
                                                                                    #define _GradientStandard _GradientStandard_Definition()
                                                                                    Gradient _GradientRedStrength_Definition()
                                                                                    {
                                                                                        Gradient g;
                                                                                        g.type = 0;
                                                                                        g.colorsLength = 5;
                                                                                        g.alphasLength = 2;
                                                                                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                                        g.colors[4] = float4(1, 0, 0, 1);
                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                        g.alphas[0] = float2(1, 0);
                                                                                        g.alphas[1] = float2(1, 1);
                                                                                        g.alphas[2] = float2(0, 0);
                                                                                        g.alphas[3] = float2(0, 0);
                                                                                        g.alphas[4] = float2(0, 0);
                                                                                        g.alphas[5] = float2(0, 0);
                                                                                        g.alphas[6] = float2(0, 0);
                                                                                        g.alphas[7] = float2(0, 0);
                                                                                        return g;
                                                                                    }
                                                                                    #define _GradientRedStrength _GradientRedStrength_Definition()
                                                                                    Gradient _GradientWhiteHot_Definition()
                                                                                    {
                                                                                        Gradient g;
                                                                                        g.type = 0;
                                                                                        g.colorsLength = 3;
                                                                                        g.alphasLength = 2;
                                                                                        g.colors[0] = float4(0, 0, 0, 0);
                                                                                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                                        g.colors[2] = float4(1, 1, 1, 1);
                                                                                        g.colors[3] = float4(0, 0, 0, 0);
                                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                        g.alphas[0] = float2(1, 0);
                                                                                        g.alphas[1] = float2(1, 1);
                                                                                        g.alphas[2] = float2(0, 0);
                                                                                        g.alphas[3] = float2(0, 0);
                                                                                        g.alphas[4] = float2(0, 0);
                                                                                        g.alphas[5] = float2(0, 0);
                                                                                        g.alphas[6] = float2(0, 0);
                                                                                        g.alphas[7] = float2(0, 0);
                                                                                        return g;
                                                                                    }
                                                                                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                                                    Gradient _GradientBlackHot_Definition()
                                                                                    {
                                                                                        Gradient g;
                                                                                        g.type = 0;
                                                                                        g.colorsLength = 4;
                                                                                        g.alphasLength = 2;
                                                                                        g.colors[0] = float4(1, 1, 1, 0);
                                                                                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                                        g.colors[3] = float4(0, 0, 0, 1);
                                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                        g.alphas[0] = float2(1, 0);
                                                                                        g.alphas[1] = float2(1, 1);
                                                                                        g.alphas[2] = float2(0, 0);
                                                                                        g.alphas[3] = float2(0, 0);
                                                                                        g.alphas[4] = float2(0, 0);
                                                                                        g.alphas[5] = float2(0, 0);
                                                                                        g.alphas[6] = float2(0, 0);
                                                                                        g.alphas[7] = float2(0, 0);
                                                                                        return g;
                                                                                    }
                                                                                    #define _GradientBlackHot _GradientBlackHot_Definition()

                                                                                    // Graph Includes
                                                                                    // GraphIncludes: <None>

                                                                                    // -- Property used by ScenePickingPass
                                                                                    #ifdef SCENEPICKINGPASS
                                                                                    float4 _SelectionID;
                                                                                    #endif

                                                                                    // -- Properties used by SceneSelectionPass
                                                                                    #ifdef SCENESELECTIONPASS
                                                                                    int _ObjectId;
                                                                                    int _PassValue;
                                                                                    #endif

                                                                                    // Graph Functions

                                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                                    {
                                                                                        Out = A / B;
                                                                                    }

                                                                                    // Custom interpolators pre vertex
                                                                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                                                    // Graph Vertex
                                                                                    struct VertexDescription
                                                                                    {
                                                                                        float3 Position;
                                                                                        float3 Normal;
                                                                                        float3 Tangent;
                                                                                    };

                                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                    {
                                                                                        VertexDescription description = (VertexDescription)0;
                                                                                        description.Position = IN.ObjectSpacePosition;
                                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                                        return description;
                                                                                    }

                                                                                    // Custom interpolators, pre surface
                                                                                    #ifdef FEATURES_GRAPH_VERTEX
                                                                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                                                    {
                                                                                    return output;
                                                                                    }
                                                                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                                                    #endif

                                                                                    // Graph Pixel
                                                                                    struct SurfaceDescription
                                                                                    {
                                                                                        float Alpha;
                                                                                    };

                                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                    {
                                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                        return surface;
                                                                                    }

                                                                                    // --------------------------------------------------
                                                                                    // Build Graph Inputs
                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                    #define VFX_SRP_ATTRIBUTES Attributes
                                                                                    #define VFX_SRP_VARYINGS Varyings
                                                                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                                                    #endif
                                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                    {
                                                                                        VertexDescriptionInputs output;
                                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                                        return output;
                                                                                    }
                                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                    {
                                                                                        SurfaceDescriptionInputs output;
                                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                                                    #endif







                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                    #else
                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                    #endif
                                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                            return output;
                                                                                    }

                                                                                    // --------------------------------------------------
                                                                                    // Main

                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                                                                                    // --------------------------------------------------
                                                                                    // Visual Effect Vertex Invocations
                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                                                    #endif

                                                                                    ENDHLSL
                                                                                    }
                                                                                    Pass
                                                                                    {
                                                                                        Name "ScenePickingPass"
                                                                                        Tags
                                                                                        {
                                                                                            "LightMode" = "Picking"
                                                                                        }

                                                                                        // Render State
                                                                                        Cull Back

                                                                                        // Debug
                                                                                        // <None>

                                                                                        // --------------------------------------------------
                                                                                        // Pass

                                                                                        HLSLPROGRAM

                                                                                        // Pragmas
                                                                                        #pragma target 2.0
                                                                                        #pragma only_renderers gles gles3 glcore d3d11
                                                                                        #pragma multi_compile_instancing
                                                                                        #pragma vertex vert
                                                                                        #pragma fragment frag

                                                                                        // DotsInstancingOptions: <None>
                                                                                        // HybridV1InjectedBuiltinProperties: <None>

                                                                                        // Keywords
                                                                                        // PassKeywords: <None>
                                                                                        // GraphKeywords: <None>

                                                                                        // Defines

                                                                                        #define ATTRIBUTES_NEED_NORMAL
                                                                                        #define ATTRIBUTES_NEED_TANGENT
                                                                                        #define FEATURES_GRAPH_VERTEX
                                                                                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                        #define SHADERPASS SHADERPASS_DEPTHONLY
                                                                                        #define SCENEPICKINGPASS 1
                                                                                        #define ALPHA_CLIP_THRESHOLD 1
                                                                                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                                                        // custom interpolator pre-include
                                                                                        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                                                        // Includes
                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                                                        // --------------------------------------------------
                                                                                        // Structs and Packing

                                                                                        // custom interpolators pre packing
                                                                                        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                                                        struct Attributes
                                                                                        {
                                                                                             float3 positionOS : POSITION;
                                                                                             float3 normalOS : NORMAL;
                                                                                             float4 tangentOS : TANGENT;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                             uint instanceID : INSTANCEID_SEMANTIC;
                                                                                            #endif
                                                                                        };
                                                                                        struct Varyings
                                                                                        {
                                                                                             float4 positionCS : SV_POSITION;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                            #endif
                                                                                        };
                                                                                        struct SurfaceDescriptionInputs
                                                                                        {
                                                                                        };
                                                                                        struct VertexDescriptionInputs
                                                                                        {
                                                                                             float3 ObjectSpaceNormal;
                                                                                             float3 ObjectSpaceTangent;
                                                                                             float3 ObjectSpacePosition;
                                                                                        };
                                                                                        struct PackedVaryings
                                                                                        {
                                                                                             float4 positionCS : SV_POSITION;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                             uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                            #endif
                                                                                        };

                                                                                        PackedVaryings PackVaryings(Varyings input)
                                                                                        {
                                                                                            PackedVaryings output;
                                                                                            ZERO_INITIALIZE(PackedVaryings, output);
                                                                                            output.positionCS = input.positionCS;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            output.instanceID = input.instanceID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            output.cullFace = input.cullFace;
                                                                                            #endif
                                                                                            return output;
                                                                                        }

                                                                                        Varyings UnpackVaryings(PackedVaryings input)
                                                                                        {
                                                                                            Varyings output;
                                                                                            output.positionCS = input.positionCS;
                                                                                            #if UNITY_ANY_INSTANCING_ENABLED
                                                                                            output.instanceID = input.instanceID;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                            #endif
                                                                                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                            #endif
                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            output.cullFace = input.cullFace;
                                                                                            #endif
                                                                                            return output;
                                                                                        }


                                                                                        // --------------------------------------------------
                                                                                        // Graph

                                                                                        // Graph Properties
                                                                                        CBUFFER_START(UnityPerMaterial)
                                                                                        float _Temperature;
                                                                                        CBUFFER_END

                                                                                            // Object and Global properties
                                                                                            Gradient _GradientStandard_Definition()
                                                                                            {
                                                                                                Gradient g;
                                                                                                g.type = 0;
                                                                                                g.colorsLength = 6;
                                                                                                g.alphasLength = 2;
                                                                                                g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                                                g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                                                g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                                                g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                                                g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                                                g.colors[5] = float4(1, 1, 1, 1);
                                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                                g.alphas[0] = float2(1, 0);
                                                                                                g.alphas[1] = float2(1, 1);
                                                                                                g.alphas[2] = float2(0, 0);
                                                                                                g.alphas[3] = float2(0, 0);
                                                                                                g.alphas[4] = float2(0, 0);
                                                                                                g.alphas[5] = float2(0, 0);
                                                                                                g.alphas[6] = float2(0, 0);
                                                                                                g.alphas[7] = float2(0, 0);
                                                                                                return g;
                                                                                            }
                                                                                            #define _GradientStandard _GradientStandard_Definition()
                                                                                            Gradient _GradientRedStrength_Definition()
                                                                                            {
                                                                                                Gradient g;
                                                                                                g.type = 0;
                                                                                                g.colorsLength = 5;
                                                                                                g.alphasLength = 2;
                                                                                                g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                                                g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                                                g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                                                g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                                                g.colors[4] = float4(1, 0, 0, 1);
                                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                                g.alphas[0] = float2(1, 0);
                                                                                                g.alphas[1] = float2(1, 1);
                                                                                                g.alphas[2] = float2(0, 0);
                                                                                                g.alphas[3] = float2(0, 0);
                                                                                                g.alphas[4] = float2(0, 0);
                                                                                                g.alphas[5] = float2(0, 0);
                                                                                                g.alphas[6] = float2(0, 0);
                                                                                                g.alphas[7] = float2(0, 0);
                                                                                                return g;
                                                                                            }
                                                                                            #define _GradientRedStrength _GradientRedStrength_Definition()
                                                                                            Gradient _GradientWhiteHot_Definition()
                                                                                            {
                                                                                                Gradient g;
                                                                                                g.type = 0;
                                                                                                g.colorsLength = 3;
                                                                                                g.alphasLength = 2;
                                                                                                g.colors[0] = float4(0, 0, 0, 0);
                                                                                                g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                                                g.colors[2] = float4(1, 1, 1, 1);
                                                                                                g.colors[3] = float4(0, 0, 0, 0);
                                                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                                g.alphas[0] = float2(1, 0);
                                                                                                g.alphas[1] = float2(1, 1);
                                                                                                g.alphas[2] = float2(0, 0);
                                                                                                g.alphas[3] = float2(0, 0);
                                                                                                g.alphas[4] = float2(0, 0);
                                                                                                g.alphas[5] = float2(0, 0);
                                                                                                g.alphas[6] = float2(0, 0);
                                                                                                g.alphas[7] = float2(0, 0);
                                                                                                return g;
                                                                                            }
                                                                                            #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                                                            Gradient _GradientBlackHot_Definition()
                                                                                            {
                                                                                                Gradient g;
                                                                                                g.type = 0;
                                                                                                g.colorsLength = 4;
                                                                                                g.alphasLength = 2;
                                                                                                g.colors[0] = float4(1, 1, 1, 0);
                                                                                                g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                                                g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                                                g.colors[3] = float4(0, 0, 0, 1);
                                                                                                g.colors[4] = float4(0, 0, 0, 0);
                                                                                                g.colors[5] = float4(0, 0, 0, 0);
                                                                                                g.colors[6] = float4(0, 0, 0, 0);
                                                                                                g.colors[7] = float4(0, 0, 0, 0);
                                                                                                g.alphas[0] = float2(1, 0);
                                                                                                g.alphas[1] = float2(1, 1);
                                                                                                g.alphas[2] = float2(0, 0);
                                                                                                g.alphas[3] = float2(0, 0);
                                                                                                g.alphas[4] = float2(0, 0);
                                                                                                g.alphas[5] = float2(0, 0);
                                                                                                g.alphas[6] = float2(0, 0);
                                                                                                g.alphas[7] = float2(0, 0);
                                                                                                return g;
                                                                                            }
                                                                                            #define _GradientBlackHot _GradientBlackHot_Definition()

                                                                                            // Graph Includes
                                                                                            // GraphIncludes: <None>

                                                                                            // -- Property used by ScenePickingPass
                                                                                            #ifdef SCENEPICKINGPASS
                                                                                            float4 _SelectionID;
                                                                                            #endif

                                                                                            // -- Properties used by SceneSelectionPass
                                                                                            #ifdef SCENESELECTIONPASS
                                                                                            int _ObjectId;
                                                                                            int _PassValue;
                                                                                            #endif

                                                                                            // Graph Functions

                                                                                            void Unity_Divide_float(float A, float B, out float Out)
                                                                                            {
                                                                                                Out = A / B;
                                                                                            }

                                                                                            // Custom interpolators pre vertex
                                                                                            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                                                            // Graph Vertex
                                                                                            struct VertexDescription
                                                                                            {
                                                                                                float3 Position;
                                                                                                float3 Normal;
                                                                                                float3 Tangent;
                                                                                            };

                                                                                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                            {
                                                                                                VertexDescription description = (VertexDescription)0;
                                                                                                description.Position = IN.ObjectSpacePosition;
                                                                                                description.Normal = IN.ObjectSpaceNormal;
                                                                                                description.Tangent = IN.ObjectSpaceTangent;
                                                                                                return description;
                                                                                            }

                                                                                            // Custom interpolators, pre surface
                                                                                            #ifdef FEATURES_GRAPH_VERTEX
                                                                                            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                                                            {
                                                                                            return output;
                                                                                            }
                                                                                            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                                                            #endif

                                                                                            // Graph Pixel
                                                                                            struct SurfaceDescription
                                                                                            {
                                                                                                float Alpha;
                                                                                            };

                                                                                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                            {
                                                                                                SurfaceDescription surface = (SurfaceDescription)0;
                                                                                                float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                                                float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                                Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                                                surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                                return surface;
                                                                                            }

                                                                                            // --------------------------------------------------
                                                                                            // Build Graph Inputs
                                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                                            #define VFX_SRP_ATTRIBUTES Attributes
                                                                                            #define VFX_SRP_VARYINGS Varyings
                                                                                            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                                                            #endif
                                                                                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                            {
                                                                                                VertexDescriptionInputs output;
                                                                                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                                output.ObjectSpaceNormal = input.normalOS;
                                                                                                output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                                                output.ObjectSpacePosition = input.positionOS;

                                                                                                return output;
                                                                                            }
                                                                                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                            {
                                                                                                SurfaceDescriptionInputs output;
                                                                                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                                                // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                                                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                                                            #endif







                                                                                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                            #else
                                                                                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                            #endif
                                                                                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                                    return output;
                                                                                            }

                                                                                            // --------------------------------------------------
                                                                                            // Main

                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

                                                                                            // --------------------------------------------------
                                                                                            // Visual Effect Vertex Invocations
                                                                                            #ifdef HAVE_VFX_MODIFICATION
                                                                                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                                                            #endif

                                                                                            ENDHLSL
                                                                                            }
                                                                                            Pass
                                                                                            {
                                                                                                Name "DepthNormals"
                                                                                                Tags
                                                                                                {
                                                                                                    "LightMode" = "DepthNormalsOnly"
                                                                                                }

                                                                                                // Render State
                                                                                                Cull Back
                                                                                                ZTest Always
                                                                                                ZWrite On

                                                                                                // Debug
                                                                                                // <None>

                                                                                                // --------------------------------------------------
                                                                                                // Pass

                                                                                                HLSLPROGRAM

                                                                                                // Pragmas
                                                                                                #pragma target 2.0
                                                                                                #pragma only_renderers gles gles3 glcore d3d11
                                                                                                #pragma multi_compile_instancing
                                                                                                #pragma multi_compile_fog
                                                                                                #pragma instancing_options renderinglayer
                                                                                                #pragma vertex vert
                                                                                                #pragma fragment frag

                                                                                                // DotsInstancingOptions: <None>
                                                                                                // HybridV1InjectedBuiltinProperties: <None>

                                                                                                // Keywords
                                                                                                // PassKeywords: <None>
                                                                                                // GraphKeywords: <None>

                                                                                                // Defines

                                                                                                #define ATTRIBUTES_NEED_NORMAL
                                                                                                #define ATTRIBUTES_NEED_TANGENT
                                                                                                #define VARYINGS_NEED_NORMAL_WS
                                                                                                #define FEATURES_GRAPH_VERTEX
                                                                                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                                                                                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                                                                                                #define _SURFACE_TYPE_TRANSPARENT 1
                                                                                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */


                                                                                                // custom interpolator pre-include
                                                                                                /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

                                                                                                // Includes
                                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                                                                                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

                                                                                                // --------------------------------------------------
                                                                                                // Structs and Packing

                                                                                                // custom interpolators pre packing
                                                                                                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

                                                                                                struct Attributes
                                                                                                {
                                                                                                     float3 positionOS : POSITION;
                                                                                                     float3 normalOS : NORMAL;
                                                                                                     float4 tangentOS : TANGENT;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                     uint instanceID : INSTANCEID_SEMANTIC;
                                                                                                    #endif
                                                                                                };
                                                                                                struct Varyings
                                                                                                {
                                                                                                     float4 positionCS : SV_POSITION;
                                                                                                     float3 normalWS;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                    #endif
                                                                                                };
                                                                                                struct SurfaceDescriptionInputs
                                                                                                {
                                                                                                };
                                                                                                struct VertexDescriptionInputs
                                                                                                {
                                                                                                     float3 ObjectSpaceNormal;
                                                                                                     float3 ObjectSpaceTangent;
                                                                                                     float3 ObjectSpacePosition;
                                                                                                };
                                                                                                struct PackedVaryings
                                                                                                {
                                                                                                     float4 positionCS : SV_POSITION;
                                                                                                     float3 interp0 : INTERP0;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                     uint instanceID : CUSTOM_INSTANCE_ID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                     FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                                                                                    #endif
                                                                                                };

                                                                                                PackedVaryings PackVaryings(Varyings input)
                                                                                                {
                                                                                                    PackedVaryings output;
                                                                                                    ZERO_INITIALIZE(PackedVaryings, output);
                                                                                                    output.positionCS = input.positionCS;
                                                                                                    output.interp0.xyz = input.normalWS;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    output.instanceID = input.instanceID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    output.cullFace = input.cullFace;
                                                                                                    #endif
                                                                                                    return output;
                                                                                                }

                                                                                                Varyings UnpackVaryings(PackedVaryings input)
                                                                                                {
                                                                                                    Varyings output;
                                                                                                    output.positionCS = input.positionCS;
                                                                                                    output.normalWS = input.interp0.xyz;
                                                                                                    #if UNITY_ANY_INSTANCING_ENABLED
                                                                                                    output.instanceID = input.instanceID;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                                                                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                                                                                    #endif
                                                                                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                                                                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                                                                                    #endif
                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    output.cullFace = input.cullFace;
                                                                                                    #endif
                                                                                                    return output;
                                                                                                }


                                                                                                // --------------------------------------------------
                                                                                                // Graph

                                                                                                // Graph Properties
                                                                                                CBUFFER_START(UnityPerMaterial)
                                                                                                float _Temperature;
                                                                                                CBUFFER_END

                                                                                                    // Object and Global properties
                                                                                                    Gradient _GradientStandard_Definition()
                                                                                                    {
                                                                                                        Gradient g;
                                                                                                        g.type = 0;
                                                                                                        g.colorsLength = 6;
                                                                                                        g.alphasLength = 2;
                                                                                                        g.colors[0] = float4(0, 0.005622149, 0.4823529, 0);
                                                                                                        g.colors[1] = float4(0.003317599, 0.1509434, 0, 0.1823606);
                                                                                                        g.colors[2] = float4(0.9622642, 0.4199979, 0, 0.3705959);
                                                                                                        g.colors[3] = float4(1, 0.2306142, 0, 0.5323567);
                                                                                                        g.colors[4] = float4(1, 0, 0, 0.9264668);
                                                                                                        g.colors[5] = float4(1, 1, 1, 1);
                                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                                        g.alphas[0] = float2(1, 0);
                                                                                                        g.alphas[1] = float2(1, 1);
                                                                                                        g.alphas[2] = float2(0, 0);
                                                                                                        g.alphas[3] = float2(0, 0);
                                                                                                        g.alphas[4] = float2(0, 0);
                                                                                                        g.alphas[5] = float2(0, 0);
                                                                                                        g.alphas[6] = float2(0, 0);
                                                                                                        g.alphas[7] = float2(0, 0);
                                                                                                        return g;
                                                                                                    }
                                                                                                    #define _GradientStandard _GradientStandard_Definition()
                                                                                                    Gradient _GradientRedStrength_Definition()
                                                                                                    {
                                                                                                        Gradient g;
                                                                                                        g.type = 0;
                                                                                                        g.colorsLength = 5;
                                                                                                        g.alphasLength = 2;
                                                                                                        g.colors[0] = float4(0.3867925, 0.3867925, 0.3867925, 0);
                                                                                                        g.colors[1] = float4(0.8490566, 0.2763439, 0.2763439, 0.2588235);
                                                                                                        g.colors[2] = float4(0.6981132, 0.1350124, 0.1350124, 0.4941176);
                                                                                                        g.colors[3] = float4(1, 0, 0, 0.7794156);
                                                                                                        g.colors[4] = float4(1, 0, 0, 1);
                                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                                        g.alphas[0] = float2(1, 0);
                                                                                                        g.alphas[1] = float2(1, 1);
                                                                                                        g.alphas[2] = float2(0, 0);
                                                                                                        g.alphas[3] = float2(0, 0);
                                                                                                        g.alphas[4] = float2(0, 0);
                                                                                                        g.alphas[5] = float2(0, 0);
                                                                                                        g.alphas[6] = float2(0, 0);
                                                                                                        g.alphas[7] = float2(0, 0);
                                                                                                        return g;
                                                                                                    }
                                                                                                    #define _GradientRedStrength _GradientRedStrength_Definition()
                                                                                                    Gradient _GradientWhiteHot_Definition()
                                                                                                    {
                                                                                                        Gradient g;
                                                                                                        g.type = 0;
                                                                                                        g.colorsLength = 3;
                                                                                                        g.alphasLength = 2;
                                                                                                        g.colors[0] = float4(0, 0, 0, 0);
                                                                                                        g.colors[1] = float4(0.6603774, 0.6603774, 0.6603774, 0.4823529);
                                                                                                        g.colors[2] = float4(1, 1, 1, 1);
                                                                                                        g.colors[3] = float4(0, 0, 0, 0);
                                                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                                        g.alphas[0] = float2(1, 0);
                                                                                                        g.alphas[1] = float2(1, 1);
                                                                                                        g.alphas[2] = float2(0, 0);
                                                                                                        g.alphas[3] = float2(0, 0);
                                                                                                        g.alphas[4] = float2(0, 0);
                                                                                                        g.alphas[5] = float2(0, 0);
                                                                                                        g.alphas[6] = float2(0, 0);
                                                                                                        g.alphas[7] = float2(0, 0);
                                                                                                        return g;
                                                                                                    }
                                                                                                    #define _GradientWhiteHot _GradientWhiteHot_Definition()
                                                                                                    Gradient _GradientBlackHot_Definition()
                                                                                                    {
                                                                                                        Gradient g;
                                                                                                        g.type = 0;
                                                                                                        g.colorsLength = 4;
                                                                                                        g.alphasLength = 2;
                                                                                                        g.colors[0] = float4(1, 1, 1, 0);
                                                                                                        g.colors[1] = float4(0.1698113, 0.1698113, 0.1698113, 0.2117647);
                                                                                                        g.colors[2] = float4(0.0754717, 0.0754717, 0.0754717, 0.4588235);
                                                                                                        g.colors[3] = float4(0, 0, 0, 1);
                                                                                                        g.colors[4] = float4(0, 0, 0, 0);
                                                                                                        g.colors[5] = float4(0, 0, 0, 0);
                                                                                                        g.colors[6] = float4(0, 0, 0, 0);
                                                                                                        g.colors[7] = float4(0, 0, 0, 0);
                                                                                                        g.alphas[0] = float2(1, 0);
                                                                                                        g.alphas[1] = float2(1, 1);
                                                                                                        g.alphas[2] = float2(0, 0);
                                                                                                        g.alphas[3] = float2(0, 0);
                                                                                                        g.alphas[4] = float2(0, 0);
                                                                                                        g.alphas[5] = float2(0, 0);
                                                                                                        g.alphas[6] = float2(0, 0);
                                                                                                        g.alphas[7] = float2(0, 0);
                                                                                                        return g;
                                                                                                    }
                                                                                                    #define _GradientBlackHot _GradientBlackHot_Definition()

                                                                                                    // Graph Includes
                                                                                                    // GraphIncludes: <None>

                                                                                                    // -- Property used by ScenePickingPass
                                                                                                    #ifdef SCENEPICKINGPASS
                                                                                                    float4 _SelectionID;
                                                                                                    #endif

                                                                                                    // -- Properties used by SceneSelectionPass
                                                                                                    #ifdef SCENESELECTIONPASS
                                                                                                    int _ObjectId;
                                                                                                    int _PassValue;
                                                                                                    #endif

                                                                                                    // Graph Functions

                                                                                                    void Unity_Divide_float(float A, float B, out float Out)
                                                                                                    {
                                                                                                        Out = A / B;
                                                                                                    }

                                                                                                    // Custom interpolators pre vertex
                                                                                                    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                                                                                                    // Graph Vertex
                                                                                                    struct VertexDescription
                                                                                                    {
                                                                                                        float3 Position;
                                                                                                        float3 Normal;
                                                                                                        float3 Tangent;
                                                                                                    };

                                                                                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                                                                                    {
                                                                                                        VertexDescription description = (VertexDescription)0;
                                                                                                        description.Position = IN.ObjectSpacePosition;
                                                                                                        description.Normal = IN.ObjectSpaceNormal;
                                                                                                        description.Tangent = IN.ObjectSpaceTangent;
                                                                                                        return description;
                                                                                                    }

                                                                                                    // Custom interpolators, pre surface
                                                                                                    #ifdef FEATURES_GRAPH_VERTEX
                                                                                                    Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                                                                                                    {
                                                                                                    return output;
                                                                                                    }
                                                                                                    #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                                                                                                    #endif

                                                                                                    // Graph Pixel
                                                                                                    struct SurfaceDescription
                                                                                                    {
                                                                                                        float Alpha;
                                                                                                    };

                                                                                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                                                                                    {
                                                                                                        SurfaceDescription surface = (SurfaceDescription)0;
                                                                                                        float _Property_d928e1a37be64a1597956d18edfa7bea_Out_0 = _Temperature;
                                                                                                        float _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                                        Unity_Divide_float(_Property_d928e1a37be64a1597956d18edfa7bea_Out_0, 100, _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2);
                                                                                                        surface.Alpha = _Divide_87df39758d49449b9b7f9c3449bbd0ff_Out_2;
                                                                                                        return surface;
                                                                                                    }

                                                                                                    // --------------------------------------------------
                                                                                                    // Build Graph Inputs
                                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                                    #define VFX_SRP_ATTRIBUTES Attributes
                                                                                                    #define VFX_SRP_VARYINGS Varyings
                                                                                                    #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
                                                                                                    #endif
                                                                                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                                                                                    {
                                                                                                        VertexDescriptionInputs output;
                                                                                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                                                                                        output.ObjectSpaceNormal = input.normalOS;
                                                                                                        output.ObjectSpaceTangent = input.tangentOS.xyz;
                                                                                                        output.ObjectSpacePosition = input.positionOS;

                                                                                                        return output;
                                                                                                    }
                                                                                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                                                                                    {
                                                                                                        SurfaceDescriptionInputs output;
                                                                                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                                        // FragInputs from VFX come from two places: Interpolator or CBuffer.
                                                                                                        /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

                                                                                                    #endif







                                                                                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                                                                                    #else
                                                                                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                                                                                    #endif
                                                                                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                                                                                            return output;
                                                                                                    }

                                                                                                    // --------------------------------------------------
                                                                                                    // Main

                                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                                                                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

                                                                                                    // --------------------------------------------------
                                                                                                    // Visual Effect Vertex Invocations
                                                                                                    #ifdef HAVE_VFX_MODIFICATION
                                                                                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
                                                                                                    #endif

                                                                                                    ENDHLSL
                                                                                                    }
                                                    }
                                                        CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
                                                                                                        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
                                                                                                        FallBack "Hidden/Shader Graph/FallbackError"
}