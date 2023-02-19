// Written by Charlie
// Edited by Henrik

Shader "CharlieCustom/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [Toggle(_USE_NORMALS)] _UseNormalMap ("Use Normal Map", float) = 1
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "Bump" {}
        [Toggle] _UseMetallicMap ("Use Metallic Map", float) = 1
        [NoScaleOffset] _MetallicMap ("Metallic Map", 2D) = "White" {}
        [Toggle] _UseOcclusionMap ("Use Occlusion Map", float) = 1
        [NoScaleOffset] _OcclusionMap ("Occlusion Map", 2D) = "White" {}
        _DissolveMap ("Dissolve Map", 2D) = "white" {}
        _Cutoff("Dissolve Amount", Range(0,1)) = 1.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Occlusion("Occlusion", Range(0,1)) = 1
        _Metallic("Metallic", Range(0,1)) = 0.0
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _EdgeWidth("Edge Thickness", Range(0.01,0.2)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #pragma multi_compile __ _USE_NORMALS

        sampler2D _MainTex;
        sampler2D _DissolveMap;
        float _Cutoff;
        float4 _EdgeColor;
        float _EdgeWidth;

        // Maps
        float _UseNormalMap;
        float _UseMetallicMap;
        float _UseOcclusionMap;
        sampler2D _NormalMap;
        sampler2D _MetallicMap;
        sampler2D _OcclusionMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        half _Occlusion;
        fixed4 _Color;

        #pragma instancing_options assumeuniformscaling

        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            
            #ifdef _USE_NORMALS
            o.Normal = UnpackNormal(tex2D( _NormalMap, IN.uv_MainTex));      // tangent space normal, if written
            #endif

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 dissolveMap = tex2D(_DissolveMap, IN.uv_MainTex);
            fixed4 metallicMap = tex2D(_MetallicMap, IN.uv_MainTex);
            fixed4 occlusionMap = tex2D(_OcclusionMap, IN.uv_MainTex);

            o.Albedo = c.rgb;
            clip(dissolveMap.a - _Cutoff);


            o.Metallic = _UseMetallicMap < 0 ? _Metallic : metallicMap;
            o.Occlusion = _UseOcclusionMap < 0 ? _Occlusion : occlusionMap;

            float edge = step(dissolveMap, _Cutoff +  _EdgeWidth);
            o.Smoothness = _Glossiness;
            o.Emission = _EdgeColor * edge;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
