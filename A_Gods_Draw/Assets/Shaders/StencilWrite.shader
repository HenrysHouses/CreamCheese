Shader "Unlit/StencilWrite"
{
    Properties
    {
        _Layer ("Write to stencil", int) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }

        ZWrite Off // Off needed for transparency. this shader does not write to the debth buffer
        ZTest LEqual // LEqual - Default (under), GEqual - only behind something, Always - Always above
        Blend SrcAlpha OneMinusSrcAlpha

        // #pragma multi_compile __ _USE_OUTLINEFRESNEL
    


        Pass
        {

            Stencil {
                Ref [_Layer]
                Pass Replace
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

        // Pass
        // {
        //     Stencil {
        //         Ref [_Layer]
        //         Comp NotEqual
        //         Pass Keep
        //     }

        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     // make fog work

        //     #include "UnityCG.cginc"
        //     #include "Assets/Shaders/HHMacros.cginc"

        //     struct MeshData
        //     {
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL;
        //         float2 uv : TEXCOORD0;
        //     };

        //     struct Interpolators
        //     {
        //         float4 pos : SV_POSITION;
        //         float2 uv : TEXCOORD0;
        //         float3 wPos : TEXCOORD2;
        //         float3 worldNormal : NORMAL;
        //         DECLARE_SCRNPOS_COORDS(1)
        //     };

        //     sampler2D _MainTex;
        //     float4 _MainTex_ST;
        //     float4 _Color;
        //     float _Alpha;
        //     float _Alpha1;
        //     float _Alpha2;
        //     float _Size;
        //     float _ScreenResolution;

        //     Interpolators vert (MeshData v)
        //     {
        //         Interpolators o;
        //         o.pos = UnityObjectToClipPos(v.vertex + v.normal * _Size);
        //         o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //         o.wPos = mul( unity_ObjectToWorld, v.vertex); 
        //         o.worldNormal = UnityObjectToWorldNormal( v.normal ); // mesh normals
        //         TRANSFER_SCREENPOSITION(o)
        //         return o;
        //     }

        //     fixed4 frag (Interpolators i) : SV_Target
        //     {
        //         float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); // handles when the light is different types
        //         float3 V = normalize(_WorldSpaceCameraPos - i.wPos );
        //         float3 N = normalize(i.worldNormal);
        //         float3 H = normalize(L + V);
             
        //         // sample the texture
        //         float depth = -UnityObjectToViewPos(float3(0,0,0)).z;
        //         float2 scrUV = GetScrSpaceUV(i.scrPos, _ScreenParams, _ScreenResolution);
        //         fixed4 col = tex2D(_MainTex, _MainTex_ST.xy * scrUV * depth);
            
        //         #ifdef _USE_OUTLINEFRESNEL
        //             float fresnel = saturate(dot(V * _Alpha, N * _Alpha1));
        //             fresnel = 1-step(fresnel, _Alpha2);
        //             return float4(fresnel.xxx, 1);
        //             col.w = fresnel;
        //         #endif

        //         return col * _Color;
        //     }
        //     ENDCG
        // }
    }
}