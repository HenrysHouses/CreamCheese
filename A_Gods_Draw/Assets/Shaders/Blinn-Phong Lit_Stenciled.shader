Shader "HenryCustom/Lit/Blinn-Phong_V2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)
        _ColorRimLight ("fresnel Color", Color) = (1,1,1,1)
        [Toggle] _RimLightToggle ("fresnel Toggle", float) = 1
        _RimLightIntensity ("fresnel Strength", Range(0, 2)) = 1
        _Gloss ("Gloss", Range(0, 1)) = 1
        _MaskLayer ("Mask Layer", int) = 1 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Stencil {
            Ref [_MaskLayer]
            Comp Equal
            Pass Keep
        }

        // Base Pass
        Pass // always for directional light // cant have point light
        {
            Tags{"Lightmode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "HHLighting.cginc"

            ENDCG
        }

        // Add Pass
        Pass
        {
            Tags{"Lightmode" = "ForwardAdd"}
            Blend One One // src*1 + dst*1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd
            
            #include "HHLighting.cginc"

            ENDCG
        }

        // shadow caster rendering pass, implemented manually
        // using macros from UnityCG.cginc
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            Name "ShadowCast"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
