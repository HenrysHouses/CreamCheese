// Written by Henrik

Shader "Custom/CardArt"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ArtColor ("Color", Color) = (1,1,1,1)
        _MainTex ("Background", 2D) = "white" {}
        [NoScaleOffset] _ArtTex ("Art", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "Bump" {}
        [Toggle(_USE_EMISSION)] _UseEmission ("Emission", float) = 0
        [NoScaleOffset] _EmissionTex ("Art", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission Color", color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        

        _DissolveMap ("Dissolve Map", 2D) = "white" {}
        _Cutoff("Dissolve Amount", Range(0,1)) = 1.0
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _EdgeWidth("Edge Thickness", Range(0.01,0.2)) = 0.01
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

        // Dissolve
        sampler2D _DissolveMap;
        float _Cutoff;
        float4 _EdgeColor;
        float _EdgeWidth;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ArtColor;
        fixed4 _EmissionColor;

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
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = FinalTexture.a;

            fixed4 dissolveMap = tex2D(_DissolveMap, IN.uv_MainTex);

            clip(dissolveMap.a - _Cutoff);
            float edge = step(dissolveMap, _Cutoff +  _EdgeWidth);
            o.Emission = _EdgeColor * edge;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
