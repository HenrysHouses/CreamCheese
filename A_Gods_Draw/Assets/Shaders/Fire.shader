Shader "HenryCustom/Fire"
{
    Properties
    {
        [HDR] _MainColor ("Main Color", Color) = (1,1,1,1)
        [HDR] _SecondColor ("Second Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondTex ("Secondary Texture", 2D) = "white" {}
        _MainSaturation ("Main Texture Saturation", float) = 1
        _ScrollTex ("Scrolling Texture", 2D) = "white" {}
        _WallIntensity ("Fire Wall Distortion", float) = 1
        _FireWallTex ("Fire Wall Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", float) = 1
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float4 _SecondColor;
            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _ScrollTex;
            sampler2D _NoiseTex;
            sampler2D _FireWallTex;
            float4 _MainTex_ST;
            float4 _SecondTex_ST;
            float4 _ScrollTex_ST;
            float4 _NoiseTex_ST;
            float4 _FireWallTex_ST;
            float _NoiseStrength;
            float _MainSaturation;
            float _WallIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
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
                // Scroll the Noise
                float2 NoiseUv = i.uv;
                NoiseUv.y -= _Time.w * 0.1;
                fixed4 NoiseCol = tex2D(_NoiseTex, NoiseUv);
                
                // Apply noise and scrolling
                // Main Tex
                float2 MainUv = i.uv + NoiseCol.xy * _NoiseStrength;
                MainUv.x += _Time.y * 0.15;
                MainUv.y -= _NoiseStrength;
                fixed4 MainCol = tex2D(_MainTex, MainUv);

                // Secondary Tex
                float2 SecondUv = i.uv + NoiseCol.xy * _NoiseStrength;
                SecondUv.x -= _Time.y * 0.01;
                SecondUv.y -= _NoiseStrength;
                fixed4 SecondCol = tex2D(_SecondTex, SecondUv);
                
                // Wall Tex
                float2 WallUv = i.uv + NoiseCol.xy * _WallIntensity;
                WallUv.y -= _Time.w * 0.03;
                fixed4 WallCol = tex2D(_FireWallTex, WallUv);

                float2 ScrollUv = i.uv - float2(_Time.y * 0.1, _Time.y * 0.5);
                fixed4 ScrollCol = tex2D(_ScrollTex, ScrollUv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float4 FireCol = saturate((MainCol + SecondCol) * _MainSaturation );
                float4 DistortionCol = saturate(WallCol + ScrollCol);
                
                float4 CombinedCol = FireCol * DistortionCol;
                float4 col = lerp(_MainColor, _SecondColor, CombinedCol);

                // float4 OutCol = lerp(_MainColor, _SecondColor, CombinedCol); 
                return CombinedCol * col;
            }
            ENDCG
        }
    }
}
