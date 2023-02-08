Shader "CharlieCustom/Dissolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DissolveMap ("Dissolve Map", 2D) = "white" {}
        _Cutoff("Dissolve Amount", Range(0,1)) = 1.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
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

        sampler2D _MainTex;
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

        #pragma instancing_options assumeuniformscaling

        UNITY_INSTANCING_BUFFER_START(Props)

        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 dissolveMap = tex2D(_DissolveMap, IN.uv_MainTex);

            o.Albedo = c.rgb;
            clip(dissolveMap.a - _Cutoff);
            o.Metallic = _Metallic;
            float edge = step(dissolveMap, _Cutoff +  _EdgeWidth);
            o.Smoothness = _Glossiness;
            o.Emission = _EdgeColor * edge;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
