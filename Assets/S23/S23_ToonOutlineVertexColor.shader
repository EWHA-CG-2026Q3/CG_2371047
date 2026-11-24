
Shader "CG/S23_ToonOutlineVertexColor"
{
    Properties
    {
        _BaseColor ("Base Color (Tint)", Color) = (1, 1, 1, 1)
        _Ambient ("Ambient", Range(0, 1)) = 0.2
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess ("Shininess", Range(1, 256)) = 32
        _ToonSteps ("Toon Steps", Range(1, 8)) = 3
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        // Pass 1: Toon Shading + Vertex Color
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _BaseColor;
            float _Ambient;
            float4 _SpecularColor;
            float _Shininess;
            float _ToonSteps;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 color      : COLOR;      // [추가] 정점 색
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float4 color      : TEXCOORD2;  // [추가] 정점 색 전달
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionWS = TransformObjectToWorld(input.positionOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);

                output.color = input.color;     // [추가]

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float3 N = normalize(input.normalWS);

                Light mainLight = GetMainLight();
                float3 L = mainLight.direction;
                float3 V = normalize(GetCameraPositionWS() - input.positionWS);
                float3 R = reflect(-L, N);

                // Toon Shading
                float diffuse = max(0, dot(N, L));
                diffuse = floor(diffuse * _ToonSteps) / _ToonSteps;

                float specular = pow(max(0, dot(R, V)), _Shininess);

                // [추가] Vertex Color와 Base Color 결합
                float3 surfaceColor = input.color.rgb * _BaseColor.rgb;

                // [수정] Toon 명암에 정점 색 적용
                float3 color = surfaceColor * (_Ambient + diffuse * mainLight.color)
                             + _SpecularColor.rgb * specular * mainLight.color;

                return float4(color, 1);
            }

            ENDHLSL
        }

        // Pass 2: Outline
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _OutlineColor;
            float _OutlineWidth;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 pushed = input.positionOS
                              + input.normalOS * _OutlineWidth;

                output.positionCS = TransformObjectToHClip(pushed);

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}
