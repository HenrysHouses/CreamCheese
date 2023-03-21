Shader "Unlit/NoiseLayering"
{
    Properties
    {
        [KeywordEnum(Opaque, Transparent, Cutout)] _RenderType ("Render Type", int) = 0
        [KeywordEnum(Off, Back, Forward)] _Culling ("Culling", int) = 0
        [HDR] _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        [HDR] _SecondColor ("Secondary Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _SecondTex ("Texture", 2D) = "white" {}
        _BlendTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="[_RenderType]" }

        HLSLINCLUDE
            #pragma multi_compile _RENDERTYPE_OPAQUE _RENDERTYPE_TRANSPARENT _RENDERTYPE_CUTOUT
        ENDHLSL

        Cull [_Culling] // Front, Off // which sides of the object are not rendered.

        #if defined(_RENDERTYPE_TRANSPARENT) || defined(_RENDERTYPE_CUTOUT)
            ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
            ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
            Blend SrcAlpha OneMinusSrcAlpha
        #endif

        Pass
        {
            HLSLPROGRAM
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

            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _BlendTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _SecondTex_ST;
            float4 _BlendTex_ST;
            float4 _NoiseTex_ST;
            float4 _MainColor;
            float4 _SecondColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample Textures
                // Main
                float2 MainUV = (i.uv * _MainTex_ST.xy) + (_MainTex_ST.zw * _Time.yy);
                fixed4 MainCol = tex2D(_MainTex, MainUV);
                // Secondary
                float2 SecondUV = (i.uv * _SecondTex_ST.xy) + (_SecondTex_ST.zw * _Time.yy);
                fixed4 SecondCol = tex2D(_SecondTex, SecondUV);
                // Blend
                float2 BlendUV = (i.uv * _BlendTex_ST.xy) + (_BlendTex_ST.zw * _Time.yy);
                fixed4 BlendCol = tex2D(_BlendTex, BlendUV);
                // Noise
                float2 NoiseUV = (i.uv * _NoiseTex_ST.xy) + (_NoiseTex_ST.zw * _Time.yy);
                fixed4 NoiseCol = tex2D(_NoiseTex, NoiseUV);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float4 appliedTex = (MainCol + SecondCol) * (BlendCol + NoiseCol);
                float4 col = lerp(_MainColor, _SecondColor, appliedTex);
                
                return col;
            }
            ENDHLSL
        }
    }
}