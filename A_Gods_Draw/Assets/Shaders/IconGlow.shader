Shader "Unlit/GlowOutline"
{
    Properties
    {
        [HDR] _MainColor ("Main Color", Color) = (1,1,1,1)
        [HDR] _SecondColor ("Secondary Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondTex ("Secondary Texture", 2D) = "white" {}
        _BlendTex ("Blend Texture", 2D) = "white" {}
        [KeywordEnum(Off, Back, Forward)] _Culling ("Culling", int) = 0
        
        [Header(Glow)]
        _Origin ("Glow Origin", vector) = (0,0,0,0)
        _UVOffset ("Uv Offset", Float) = 1
        _Size ("Mesh Size", float) = 1
        _Intensity ("Intensity", float) = 1
        _RadialSpeed ("Radial Speed", Range(-2,2)) = 1
        _ExpandSpeed ("Expand Speed", Range(-2,2)) = 1
        [NoScaleOffset] _MaskTex ("Mask", 2D) = "white" {}
        _IconTexturte ("Icon", 2D) = "white" {}
        _IconTransparency ("Icon Transparency", Range(0,1)) = 1
        _IconSize ("Icon Size", float) = 1
        _GlowPower ("Intensity", Range(0,2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Cull [_Culling] // Front, Off // which sides of the object are not rendered.
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

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _BlendTex;
            float4 _MainTex_ST;
            float4 _SecondTex_ST;
            float4 _BlendTex_ST;
            float2 _Origin;
            float _Size;
            float _Intensity;
            float _RadialSpeed;
            float _ExpandSpeed;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float4 _MainColor;
            float4 _SecondColor;
            sampler2D _IconTexturte;
            float4 _IconTexturte_ST;
            float _IconSize;
            float _UVOffset;
            float _GlowPower;
            float _IconTransparency;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex + v.normal * _Size);
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                // o.wPos = mul( unity_ObjectToWorld, v.vertex); 
                // o.worldNormal = UnityObjectToWorldNormal( v.normal ); // mesh normals
                // TRANSFER_SCREENPOSITION(o)
                return o;
            }

            // Rotation works up to 180 degrees
            float RadialGradient(float2 UV, float rotation)
            {
                // Generating gradient
                float angle = atan2(UV.y, UV.x); // compute angle in radians

                // Rotate angle
                rotation = rotation == 0 ? 0.0001 : rotation;

                float correctionBias = rotation > 0 ? 0.5/rotation : 0;


                angle += radians(rotation);
                float rotationCorrection = angle < 0.0 ? 3.141592 * 2 : 0;
                angle += correctionBias == 0 ? 0: rotationCorrection;
                float _Offset = rotation * correctionBias; // Corrects the gradient range when rotated
                float gradient = (angle + 3.141592) / (2.0 * 3.141592) - _Offset; // convert angle to a value between 0 and 1 
                return gradient;
            }

            float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
            {
                return ((value - inputMin) / (inputMax - inputMin)) * (outputMax - outputMin) + outputMin;
            }


            fixed4 frag (Interpolators i) : SV_Target
            {
                float2 centredUV = (i.uv - _Origin) * _UVOffset;
                float radialGradient = RadialGradient(centredUV, 0);
                float circleGradient = length(centredUV);
                float2 _radialUV = float2(radialGradient, circleGradient);
                // different animation speeds for textures
                float2 mainOffset = float2(_Time.y * _RadialSpeed, _Time.y * _ExpandSpeed);
                float2 secondOffset = float2(_Time.y * _RadialSpeed, _Time.y * _ExpandSpeed * 0.75);
                float2 blendOffset = float2(_Time.y * _RadialSpeed, _Time.y * _ExpandSpeed) * -1;

                fixed4 mainCol = tex2D(_MainTex, _radialUV * _MainTex_ST.xy + mainOffset);
                fixed4 secondCol = tex2D(_SecondTex, _radialUV * _SecondTex_ST.xy + secondOffset);
                fixed4 blendCol = tex2D(_BlendTex, _radialUV * _BlendTex_ST.xy + blendOffset);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                float col = (mainCol + secondCol) * blendCol * mask.a * _Intensity; 

                float fade = length(i.uv - _Origin) * _GlowPower;

                float4 outColor = float4(lerp(_MainColor.rgb, _SecondColor.rgb, col), col);
                outColor.a *= mask * fade;

                float iconMask = step(circleGradient, _IconSize);

                centredUV += float2(0.5, 0.5) + _IconTexturte_ST.zw;
                centredUV *= _IconTexturte_ST.xy;
                fixed4 IconDisplacement = tex2D(_SecondTex, i.uv + secondOffset * 0.2) * 0.025;
                // centredUV = RotateUV(centredUV, radians(90));
                fixed4 IconCol = tex2D(_IconTexturte, centredUV + IconDisplacement - float2(0.018,0.018) );
                IconCol.a = _IconTransparency;

                fixed4 mainCol2 = tex2D(_MainTex, i.uv + mainOffset);
                fixed4 secondCol2 = tex2D(_SecondTex, i.uv + secondOffset);
                fixed4 blendCol2 = tex2D(_BlendTex, i.uv + blendOffset);
                float col2 =(mainCol2 + secondCol2) * blendCol2;
                fixed4 combinedCol = fixed4(lerp(_MainColor.rgb, _SecondColor.rgb, col2), col2);
                combinedCol = lerp(IconCol, combinedCol, _GlowPower*0.5);
                combinedCol.a *= IconCol;
                // return combinedCol;
                // IconCol = lerp(IconCol, combinedCol, IconCol);
                // IconCol.a *= combinedCol.a;


                return outColor + combinedCol * iconMask; 
            }
            ENDCG
        }
    }
}
