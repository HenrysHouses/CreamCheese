Shader "HenryCustom/NoiseLayering"
{
    Properties
    {
        // [Header(Rendering Settings)]
        // [KeywordEnum(Opaque, Cutout, Fade, Transparent)] _RenderType ("Render Type", int) = 0
        [KeywordEnum(Off, Back, Forward)] _Culling ("Culling", int) = 0
        // [KeywordEnum(On, Off)] _ZWrite ("Z Write", int) = 0
        // [KeywordEnum(Less, LEqual, Equal, GEqual, Greater, NotEqual, Always)] _ZTest ("Z Test", int) = 1
        // [KeywordEnum(Off, BaseTransparency)] _Blend ("Blending Mode", int) = 0 // * To enable this kind of smart keyword a custom editor is needed
        
        [Header(Color)]
        [HDR] _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        [HDR] _SecondColor ("Secondary Color", Color) = (1, 1, 1, 1)
        [Header(Textures)]
        _MainTex ("Main", 2D) = "white" {}
        _SecondTex ("Secondary", 2D) = "white" {}
        _BlendTex ("Blend", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        [Toggle] _BlendMask ("Blend Mask", Float) = 1
        _MaskTex ("Mask", 2D) = "white" {}

        [Header(Shared Tiling)]
        _TilingX ("Mesh Width", Float) = 1
        _TilingY ("Mesh Length", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        // Tags { "RenderType"="[_RenderType]" }

        // Cull [_Culling] // which sides of the object are not rendered.
        // ZWrite [_ZWrite] // Off needed for transparency. this shader does not write to the debth buffer
        // ZTest [_ZTest] // https://docs.unity3d.com/Manual/SL-ZTest.html
        // Blend SrcAlpha OneMinusSrcAlpha

        Cull [_Culling] // which sides of the object are not rendered.
        ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
        ZTest LEqual // https://docs.unity3d.com/Manual/SL-ZTest.html
        Blend SrcAlpha OneMinusSrcAlpha

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
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float4 _SecondTex_ST;
            float4 _BlendTex_ST;
            float4 _NoiseTex_ST;
            float4 _MaskTex_ST;
            float4 _MainColor;
            float4 _SecondColor;
            float _TilingX;
            float _TilingY;
            float _BlendMask;

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
                float2 MainUV = (i.uv * _MainTex_ST.xy * float2(_TilingX, _TilingY)) + (_MainTex_ST.zw * _Time.yy);
                fixed4 MainCol = tex2D(_MainTex, MainUV);
                // Secondary
                float2 SecondUV = (i.uv * _SecondTex_ST.xy * float2(_TilingX, _TilingY)) + (_SecondTex_ST.zw * _Time.yy);
                fixed4 SecondCol = tex2D(_SecondTex, SecondUV);
                // Blend
                float2 BlendUV = (i.uv * _BlendTex_ST.xy * float2(_TilingX, _TilingY)) + (_BlendTex_ST.zw * _Time.yy);
                fixed4 BlendCol = tex2D(_BlendTex, BlendUV);
                // Noise
                float2 NoiseUV = (i.uv * _NoiseTex_ST.xy * float2(_TilingX, _TilingY)) + (_NoiseTex_ST.zw * _Time.yy);
                fixed4 NoiseCol = tex2D(_NoiseTex, NoiseUV);
                // Noise
                float2 MaskUV = i.uv * _MaskTex_ST.xy + (_MaskTex_ST.zw * _Time.yy);
                fixed4 MaskCol = tex2D(_MaskTex, MaskUV);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float4 appliedTex = (MainCol + SecondCol) * (BlendCol + NoiseCol);
                float4 col = lerp(_MainColor, _SecondColor, appliedTex);
                
                
                float4 appliedMask = MaskCol * (BlendCol + NoiseCol) * MaskCol.a;
                
                float4 blendMask = _BlendMask == 1 ? appliedMask : MaskCol;

                col.a = appliedTex.x * blendMask;

                return col;
            }
            ENDHLSL
        }
    }
}