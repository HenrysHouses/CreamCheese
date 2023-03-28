// Written by Henrik

Shader "Custom/HoleDarkness"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Background", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "Bump" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        [Toggle] _UseMetallic ("Use Metallic Map", int) = 0
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _MetallicMap ("Metallic Map", 2D) = "white" {}        
        // _OcclusionMap ("Occlusion Map", 2D) = "white" {}      

        [Header(Darkness Settings)]
        _UpperLimit ("Upper Limit", Float) = 1
        _LowerLimit ("Lower Limit", Float) = 0  
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        #pragma multi_compile __ _USE_EMISSION

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma instancing_options assumeuniformscaling

        
        sampler2D _MainTex;
        sampler2D _ArtTex;
        sampler2D _NormalMap;
        sampler2D _EmissionTex;
        sampler2D _MetallicMap;
        // sampler2D _OcclusionMap;

        struct Input
        {
            float2 uv_MainTex;
            float3 WorldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ArtColor;
        fixed4 _EmissionColor;
        int _UseMetallic;
        float _UpperLimit;
        float _LowerLimit;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 art = tex2D (_ArtTex, IN.uv_MainTex) * _ArtColor;
            fixed4 FinalTexture = lerp(c, art, art.w);

            o.Albedo = FinalTexture.rgb;
            o.Normal = UnpackNormal(tex2D( _NormalMap, IN.uv_MainTex));      // tangent space normal, if written
            
            #ifdef _USE_EMISSION
                fixed4 emissionTex = tex2D (_EmissionTex, IN.uv_MainTex); 
                o.Emission = emissionTex * _EmissionColor;
            #endif
            // Metallic and smoothness come from slider variables
            o.Metallic = _UseMetallic > 0 ? UnpackNormal(tex2D( _MetallicMap, IN.uv_MainTex)) : _Metallic;      // tangent space normal, if written
            // o. = UnpackNormal(tex2D( _OcclusionMap, IN.uv_MainTex));      // tangent space normal, if written
            // o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = FinalTexture.a;

            // float Darkness = smoothstep(_LowerLimit, _UpperLimit, IN.WorldPos.y);
            // float blackCol = lerp(0, 1, Darkness);
            // o.Albedo = Darkness;
            // o.Albedo *= blackCol;
            // o.Metallic *= blackCol.xxx;
            // o.Smoothness *= blackCol.xxx;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
