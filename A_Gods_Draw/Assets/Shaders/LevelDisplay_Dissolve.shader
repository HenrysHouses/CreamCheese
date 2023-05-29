// Written by Henrik
// Based on Dissolve by charlie & Henrik

Shader "HenryCustom/LevelDisplay_Dissolve"
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
        [NoSCaleOffset] _OcclusionMap ("Occlusion Map", 2D) = "White" {}
        
        [Header(Level Display)]
        _CircleOffset ("Circle Offset", Vector) = (0.5,0.5, 0, 0)
        _OuterCircle("Outer Circle Diameter", Range(0,1)) = 0.7
        _InnerCircle("Inner Circle Diameter", Range(0,1)) = 1.0
        _CircleStrength ("Strength", Range(0,1)) = 1
        _CircleFill ("Fill", Range(0,1)) = 1
        _LevelTexture ("Current Level Sprite", 2D) = "White" {}
        _Rotation ("Rotation", float) = 0

        [Header(Dissolve)]
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

        // Level Variables
        float2 _CircleOffset;
        float _OuterCircle;
        float _InnerCircle;
        float _CircleStrength;
        float _CircleFill;
        sampler2D _LevelTexture;
        float4 _LevelTexture_ST;
        float _Rotation;
        float TAU = 6.2831853071;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        half _Occlusion;
        fixed4 _Color;

        float2 RotateUV(float2 uv, float angle)
        {
            float2x2 rotationMatrix = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));
            return mul(uv - 0.5, rotationMatrix) + 0.5;
        }

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

            // Positioning circle and texture
            float2 Pos = IN.uv_MainTex - _CircleOffset.xy;
            // Generating Circle
            float _point = length(Pos);
            float outer = step(_point, _OuterCircle);
            float inner = 1-step(_point, _InnerCircle);
            float outerMask = step(_point, 2);
            float innerMask = 1-step(_point, _InnerCircle);
            float circle = outer * inner;
            float circleMask = 1-(outerMask * innerMask);
            // Generating Fill
            float angle = atan2(Pos.y, Pos.x); // compute angle in radians

            // Rotate angle
            float rotateBy = 90;
            angle += radians(rotateBy);
            angle += angle < 0.0 ? 3.141592 * 2 : 0;
            float correctionBias = 0.5/rotateBy;
            float _Offset = rotateBy * correctionBias; // Corrects the gradient range when rotated
            float gradient = (angle + 3.141592) / (2.0 * 3.141592) - _Offset; // convert angle to a value between 0 and 1 
            
            float level = step(gradient, _CircleFill);
            // Apply circle fill
            o.Albedo += level * circle;
            o.Emission += level * circle;
            
            Pos += float2(0.5, 0.5) + _LevelTexture_ST.zw;
            Pos *= _LevelTexture_ST.xy;
            Pos = RotateUV(Pos, radians(180));
            fixed4 LevelCol = tex2D(_LevelTexture, Pos) * circleMask;
            o.Albedo += lerp(o.Albedo, LevelCol, LevelCol.a);
            o.Emission += LevelCol.x * circleMask;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
