Shader "Unlit/StencilRead"
{
    Properties
    {
        _Layer ("Read from stencil buffer", int) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}

        ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
        ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
        Blend SrcAlpha OneMinusSrcAlpha

        // #pragma multi_compile __ _USE_OUTLINEFRESNEL
    
        Pass
        {
            Stencil {
                Ref [_Layer]
                Comp Equal
                Pass Keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"
            #include "Assets/Shaders/HHMacros.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}