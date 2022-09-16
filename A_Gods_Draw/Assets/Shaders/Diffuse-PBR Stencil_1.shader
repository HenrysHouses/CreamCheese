Shader "HenryCustom/Lit/Diffuse-PBR Stencil_NotEqual"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
        [NoScaleOffset] _MetallicMap ("Metallic Map", 2D) = "bump" {}
        [NoScaleOffset] _SpecularMap ("Specular Map", 2D) = "bump" {}
        [NoScaleOffset] _DiffuseIBL ("Diffuse IBL", 2D) = "Black" {}
        _GlossIntensity ("Gloss", Range(0, 1)) = 1
        _MetallicIntensity ("Metallic", Range(0, 0.2)) = 1
        _MetallicRoughness ("Roughness", Range(0, 0.2)) = 0
        [Toggle] _RimLightToggle ("fresnel Toggle", float) = 1
        _ColorRimLight ("fresnel Color", Color) = (1,1,1,1)
        _RimLightIntensity ("fresnel Strength", Range(0, 2)) = 1
        _MaskLayer ("Mask Layer", int) = 1 
    }


    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Stencil {
            Ref [_MaskLayer]
            Comp NotEqual
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
            #define IS_IN_BASEPASS
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
