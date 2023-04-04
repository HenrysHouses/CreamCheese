Shader "HenryCustom/Fire"
{
    Properties
    {
        _LUTex ("Look up Texture", 2D) = "white" {}
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondTex ("Secondary Texture", 2D) = "white" {}
        _MainSaturation ("Main Texture Saturation", float) = 1
        _SmallFireIntensity ("Small Fire Intensity", float) = 1
        _SmallFlamesTex ("Small Fires Texture", 2D) = "white" {}
        _FireWallTex ("Fire Wall Texture", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("Mask", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "bump" {}
        _NoiseStrength ("Noise Strength", float) = 1
        _Erosion ("Erosion", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        
        Cull Off // Front, Off // which sides of the object are not rendered.
        ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
        ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _LUTex;
            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _SmallFlamesTex;
            sampler2D _FireWallTex;
            sampler2D _MaskTex;
            sampler2D _NoiseTex;
            float4 _LUTex_ST;
            float4 _MainTex_ST;
            float4 _SecondTex_ST;
            float4 _SmallFlamesTex_ST;
            float _SmallFireIntensity;
            float4 _FireWallTex_ST;
            float4 _MaskTex_ST;
            float4 _NoiseTex_ST;
            float _NoiseStrength;
            float _MainSaturation;

            float _Erosion;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            float Remap (float from, float fromMin, float fromMax, float toMin,  float toMax)
            {
                float fromAbs  =  from - fromMin;
                float fromMaxAbs = fromMax - fromMin;      
                
                float normal = fromAbs / fromMaxAbs;

                float toMaxAbs = toMax - toMin;
                float toAbs = toMaxAbs * normal;

                float to = toAbs + toMin;
                
                return to;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Bottom mask
                float LinearGradient = lerp(0, 1, i.uv.y);
                float BottomMask = smoothstep(0, 0.1, LinearGradient);
                // return float4(BottomMask.xxx,1);

                // Distortion
                float2 NoiseUv = i.uv;
                NoiseUv += _Time.yy * float2(0.1, -0.25);
                fixed2 N = UnpackNormal(tex2D(_NoiseTex, NoiseUv * _NoiseTex_ST.xy)).rg * _NoiseStrength;
                float2 UvDistortion = float2(N.r, abs(N.g)); // try with and without abs
                UvDistortion *= float2(0.1, 0.5) * BottomMask; // Control vertical and horizontal distortion

                // Apply noise and scrolling
                // Flames small
                float2 ScrollUv = i.uv + _Time.yy * float2(0.1, -0.6);
                float ScrollCol = tex2D(_SmallFlamesTex, ScrollUv * _SmallFlamesTex_ST.xy).r;
                float smallFlames = smoothstep(i.uv.y-0.25+_Erosion, i.uv.y+0.25+_Erosion, ScrollCol) * BottomMask;
                // return float4(smallFlames.xxx, 1);

                // Flame Layer
                float2 WallUv = i.uv + UvDistortion.xy; // Distortion
                WallUv += _Time.yy * float2(0.0, -0.25); // Tiling
                float WallCol = tex2D(_FireWallTex, WallUv * _FireWallTex_ST.xy).r;

                // Main Tex
                float2 MainUv = i.uv + UvDistortion.xy;  
                MainUv += _Time.yy * float2(0.1, 0.0);
                fixed MainCol = tex2D(_MainTex, MainUv * _MainTex_ST.xy).r;

                // Secondary Tex
                float2 SecondUv = i.uv + UvDistortion.xy;
                SecondUv += _Time.yy * float2(-0.05, 0.0); 
                fixed SecondCol = tex2D(_SecondTex, SecondUv * _SecondTex_ST.xy).r;

                // Combine
                float4 MainFire = saturate((MainCol + SecondCol) * WallCol + smallFlames);
                // Masking
                fixed mask = saturate(MainFire.r * (tex2D(_MaskTex, i.uv).r));
                
                float3 col = tex2D(_LUTex, mask.r);

                return float4(col, mask);
            }
            ENDCG
        }
    }
}
