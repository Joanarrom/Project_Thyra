Shader "Unlit/OldCartoonCameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GrainIntensity ("Grain Intensity", Range(0, 1)) = 0.2
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.4
        _ShakeIntensity ("Shake Intensity", Range(0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
            float _GrainIntensity;
            float _VignetteIntensity;
            float _ShakeIntensity;

            v2f vert(appdata v)
            {
                v2f o;

                // Apply a slight shake effect
                float2 shake = float2(sin(_Time.y * 10.0) * _ShakeIntensity, cos(_Time.y * 10.0) * _ShakeIntensity);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.xy += shake;
                o.uv = v.uv;
                return o;
            }

            float randomNoise(float2 uv)
            {
                float noise = frac(sin(dot(uv.xy + _Time.y * 100.0, float2(12.9898, 78.233))) * 43758.5453);
                return noise;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Convert to grayscale for the old cartoon look
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                col.rgb = float3(gray, gray, gray);

                // Apply grain effect with defined noise
                float noise = randomNoise(i.uv * 500.0) * 2.0 - 1.0;
                col.rgb += noise * _GrainIntensity * 0.5;

                // Apply vignette effect
                float2 center = i.uv - 0.5;
                float vignette = 1.0 - smoothstep(0.3, 0.8, length(center) * 2.0);
                col.rgb *= lerp(1.0, vignette, _VignetteIntensity);

                return saturate(col);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
