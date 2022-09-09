Shader "HenryCustom/StencilMasking/Mask"
{
    Properties
    {
        _MaskLayer ("Mask Layer", int) = 1
        [Toggle] _ShowMask("Show Mask", int) = 0 
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="TransparentCutout"}

        ZWrite off
        ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            Stencil {
                Ref [_MaskLayer]
                Pass Replace
            }


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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _ShowMask;

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1,1,1,_ShowMask);
            }
            ENDCG
        }
    }
}
