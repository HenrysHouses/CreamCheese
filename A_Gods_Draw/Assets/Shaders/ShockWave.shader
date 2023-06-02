Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        [Toggle] _Play ("Play", int) = 0
        _PlaySpeed ("Play Speed", float) = 1
        _OffsetX ("Offset", float) = 0
        [KeywordEnum(Off, Back, Forward)] _Culling ("Culling", int) = 0
        _ColorIntensity ("Color Intensity", float) = 1
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _BottomMaskTex ("Bottom Mask", 2D) = "white" {}
        [Normal] _NormalTex ("Normal", 2D) = "bump" {}
        _DistortionStrength ("Strength", Vector) = (0.5,0,0,1)
        _NoiseOne ("Noise 1", 2D) = "white" {}
        _NoiseOneSpeed ("Speed", float) = 1
        _NoiseTwo ("Noise 1", 2D) = "white" {}
        _NoiseTwoSpeed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
        Cull [_Culling] // Front, Off // which sides of the object are not rendered.
        ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
        ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
        Blend SrcAlpha OneMinusSrcAlpha

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

            float _OffsetX;
            float _ColorIntensity;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            sampler2D _BottomMaskTex;
            sampler2D _NormalTex;
            float4 _NormalTex_ST;
            float4 _DistortionStrength;

            sampler2D _NoiseOne;
            sampler2D _NoiseTwo;
            float _NoiseOneSpeed;
            float _NoiseTwoSpeed;

            float _Play;
            float _PlaySpeed;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Play == 1 ? (_Time.y - _OffsetX) * _PlaySpeed : _OffsetX;

                float2 NoiseUV = i.uv;
                NoiseUV.x += time * _DistortionStrength.z;

                float bottomMask = tex2D(_BottomMaskTex, i.uv).r;


                fixed2 N = UnpackNormal(tex2D(_NormalTex, NoiseUV * _NormalTex_ST.xy)).rg * _DistortionStrength.w;
                float2 UvDistortion = float2(N.r, abs(N.g)); // try with and without abs
                UvDistortion *= float2(_DistortionStrength.x, _DistortionStrength.y); // Control vertical and horizontal distortion

                float2 _UV = i.uv;
                _UV.x -= time;
                _UV += UvDistortion;
                
                float2 _Noise1UV = _UV;
                _Noise1UV.x += _NoiseOneSpeed;
                float2 _Noise2UV = _UV;
                _Noise1UV.x += _NoiseTwoSpeed;

                float mask = tex2D(_MaskTex, _UV);

                float Noise1 = tex2D(_NoiseOne, _Noise1UV) * mask;
                float Noise2 = tex2D(_NoiseTwo, _Noise2UV) * mask;

                fixed4 col = tex2D(_MainTex, _UV) * _ColorIntensity;
                col.a = (mask * (Noise1 + Noise2) * bottomMask*bottomMask);
                return col;
            }
            ENDCG
        }
    }
}
