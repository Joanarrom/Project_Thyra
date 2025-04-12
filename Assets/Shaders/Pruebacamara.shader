Shader "Unlit/Pruebacamara"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.05
        _OutlineScale ("Distance Scale", Range(0, 2)) = 0.5
        _GrainIntensity ("Grain Intensity", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { 
            "Queue" = "Geometry" 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque" // Asegura que el shader se considere opaco
        }

        // Outline Pass (Se renderiza PRIMERO)
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" } // LightMode alternativo
            Cull Front // Renderiza solo las caras traseras
            ZWrite On // ¡IMPORTANTE! Escribe en el Z-Buffer

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _OutlineWidth;
            float _OutlineScale;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;
                
                // 1. Convertir a espacio mundial
                float3 worldPos = TransformObjectToWorld(v.vertex.xyz);
                float3 worldNormal = TransformObjectToWorldNormal(v.normal);
                
                // 2. Calcular dirección de la cámara
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                
                // 3. Proyectar la normal en el plano de la cámara
                float3 outlineDir = worldNormal - dot(worldNormal, viewDir) * viewDir;
                outlineDir = normalize(outlineDir) * _OutlineWidth;
                
                // 4. Ajustar grosor por distancia
                float distance = length(_WorldSpaceCameraPos - worldPos);
                outlineDir *= (1 + distance * _OutlineScale);
                
                // 5. Aplicar offset
                worldPos += outlineDir;
                
                o.vertex = TransformWorldToHClip(worldPos);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // Main Pass (Se renderiza SEGUNDO)
        Pass
        {
            Name "Main"
            Tags { "LightMode" = "UniversalForward" } // LightMode principal de URP
            ZWrite On
            Cull Back // Renderiza solo las caras frontales
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GrainIntensity; // La intensidad del grain

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Función para generar ruido basado en las coordenadas UV
            float randomNoise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                // Convertir a escala de grises para el estilo de cartoon
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                col.rgb = float3(gray, gray, gray);

                // Aplicar el efecto de grain (ruido)
                float noise = randomNoise(i.uv * 500.0 + _Time.y * 10.0) * 2.0 - 1.0; // Ruido dinámico con el tiempo
                col.rgb += noise * _GrainIntensity;

                return saturate(col); // Asegura que el valor de color esté entre 0 y 1
            }
            ENDHLSL
        }
    }
}