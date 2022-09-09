#include "UnityCG.cginc"
#include "Lighting.cginc" // needs this for lighting
#include "AutoLight.cginc"
#include "HHMacros.cginc"

struct MeshData
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

struct Interpolators
{
    float2 uv : TEXCOORD0;
    // SHADOW_COORDS(1) 
    float3 wPos : TEXCOORD2;
    float4 pos : SV_POSITION;
    float3 worldNormal : NORMAL;
    LIGHTING_COORDS(3, 4)
    DECLARE_SCRNPOS_COORDS(5)
};


sampler2D _MainTex;
float4 _MainTex_ST;
float3 _MainColor;

float _Gloss;

float4 _ColorRimLight;
float _RimLightToggle;
float _RimLightIntensity;

#ifdef USE_TOON
DECLARE_TOON_VARIABLES
    #ifdef USE_HALFTONE
    DECLARE_HALFTONE_VARIABLES
    #endif
#endif

Interpolators vert (MeshData v)
{
    Interpolators o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.worldNormal = UnityObjectToWorldNormal( v.normal );
    o.wPos = mul( unity_ObjectToWorld, v.vertex);
    TRANSFER_SHADOW(o)
    TRANSFER_VERTEX_TO_FRAGMENT(o) // actually lighting stuff so not well named
    TRANSFER_SCREENPOSITION(o)
    return o;
}

fixed4 frag (Interpolators i) : SV_Target
{
    // diffuse lighting
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); // handles when the light is different types
    float3 N = normalize(i.worldNormal);
    float3 V = normalize(_WorldSpaceCameraPos - i.wPos );
    float3 H = normalize(L + V);

    float shadow = SHADOW_ATTENUATION(i); // reading shadow data from texcoord
    float attenuation = LIGHT_ATTENUATION(i); // distance from light source
    float NdotL = max(0.0, dot(N, L));

    #if defined(USE_TOON) && defined(USE_HALFTONE)
        // Halftone Detail
        float2 _ScreenSpaceUV = i.scrPos.xy/i.scrPos.w;
        float x = _ScreenSpaceUV.x * _ScreenParams.x/(_ScreenResSize); // fixes texture stretching on widescreens
        float y = _ScreenSpaceUV.y * _ScreenParams.y/(_ScreenResSize);
        float2 _ScreenAspectUV = float2(x,y);
        float depth = -UnityObjectToViewPos(float3(0,0,0)).z;

        // Applying halftone and lighting
        float4 _PatternScreenTex = tex2D(_PatternTex, _PatternTex_ST.xy * _ScreenAspectUV * depth);
        _PatternScreenTex = 1-_PatternInvert > 0 ? _PatternScreenTex : (1-_PatternScreenTex); // if _PatternInvert Return _DotscreenTex else return 1-_DotscreenTex
        float halfTone = smoothstep(_ShadeDetail_A, _ShadeDetail_B * NdotL, _PatternScreenTex);
    #endif

    // Diffuse
    #ifdef USE_TOON
        
        #ifdef USE_HALFTONE
            float4 light = floor((NdotL * shadow * 1-halfTone)/_Detail) * _LightColor0;   
        #else
            float4 light = floor((NdotL * shadow)/_Detail) * _LightColor0;   
        #endif
        
        float4 DiffuseToon = (light + _AmbientColor) *_LightIntensity + _Brightness;
    #else    
        float3 light = saturate( dot( N, L ) ) * shadow;
        light = (light * attenuation) * _LightColor0.xyz; // saturate is the same as max(0, (dot))
    #endif

    // specular lighting
    float3 NdotL_Specular = saturate(dot(H, N)); 
    float specularExponent = exp2(_Gloss * 6 + 2 ); // might not be best to do this math in the shader
    float3 specularLight = pow( NdotL_Specular, specularExponent ) * _Gloss * attenuation; // can time with _Gloss for energy conservation aproximation // look into BRDF and inisopotic lense flaire
    specularLight *= light; // using lambert to remove specular in situations when its on the back of the model
    
    #ifdef USE_TOON
        float3 sCol = 1-_UseSpecularLightColor > 0 ? _LightColor0.xyz : (_SpecularColor * _SpecularColor.a);
        specularLight = float4(smoothstep(_GlossAntiAlias ,0.01, specularLight) * sCol * _GlossInensity, 1); // removes gradient
        specularLight = saturate(specularLight);
    #else
        specularLight *= _LightColor0.xyz;
    #endif


    float fresnel = 1-dot(V, N) * _RimLightIntensity;
    float3 rimLight = float3(fresnel, fresnel, fresnel);

    #ifdef USE_TOON
        rimLight = fresnel * NdotL_Specular;
        rimLight = smoothstep(_RimLightCutOff - _RimLightAntiAlias, _RimLightCutOff + _RimLightAntiAlias, rimLight); // removes gradient 
        rimLight *= 1-_UseSpecularLightColor > 0 ? _LightColor0.xyz : (_ColorRimLight * _ColorRimLight.a);
    #endif
    
    fresnel *= _RimLightToggle;

    #ifdef IS_IN_BASEPASS
    #endif

    fixed4 tex = tex2D(_MainTex, i.uv);

    // return GetAdditionalLightsCount() * 100;

    #ifdef USE_TOON
        float4 output =  tex * float4(_MainColor, 1) * DiffuseToon;
        output += float4(specularLight, 1) * (_GlossLayer < DiffuseToon);
        output += float4(rimLight, 1);
    #else
        float4 output = float4(tex.xyz *(light * _MainColor + specularLight + saturate(fresnel) * _ColorRimLight * _ColorRimLight.a), 1);
    #endif
    return output;
}