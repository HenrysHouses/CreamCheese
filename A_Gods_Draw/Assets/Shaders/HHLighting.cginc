#include "UnityCG.cginc"
#include "Lighting.cginc" // needs this for lighting
#include "AutoLight.cginc"
#include "HHMacros.cginc"

struct MeshData
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 tangent : TANGENT; // xyz = tangent direction, w = tangent sign
    float3 normal : NORMAL;
};

struct Interpolators
{
    float2 uv : TEXCOORD0;
    float3 wPos : TEXCOORD2;
    float3 tangent : TESSFACTOR3;
    float3 bitangent : TESSFACTOR4;
    float4 pos : SV_POSITION;
    float3 worldNormal : NORMAL;
    LIGHTING_COORDS(5, 6)
    DECLARE_SCRNPOS_COORDS(7)
};

sampler2D _NormalMap;
sampler2D _SpecularMap;

sampler2D _MetallicMap;
float _MetallicIntensity;
float _MetallicRoughness;

sampler2D _MainTex;
float4 _MainTex_ST;
float3 _MainColor;

sampler2D _DiffuseIBL;

float _GlossIntensity;

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
    o.pos = UnityObjectToClipPos(v.vertex); // vertex position
    o.uv = TRANSFORM_TEX(v.uv, _MainTex); // mesh uvs
    o.worldNormal = UnityObjectToWorldNormal( v.normal ); // mesh normals
    o.tangent = UnityWorldToObjectDir(v.tangent.xyz);
    o.bitangent = cross(o.worldNormal, o.tangent);
    o.bitangent *= v.tangent.w * unity_WorldTransformParams.w;
    o.wPos = mul( unity_ObjectToWorld, v.vertex); 
    TRANSFER_SHADOW(o)
    TRANSFER_VERTEX_TO_FRAGMENT(o) // actually lighting stuff so not well named
    TRANSFER_SCREENPOSITION(o)
    return o;
}

float2 DirToRectilinear(float3 dir) {
    float x = atan2(dir.z, dir.x)/TAU + 0.5; // 0-1;
    float y = dir.y * 0.5 + 0.5; // 0-1
    return float2(x, y);
}

fixed4 frag (Interpolators i) : SV_Target
{
    float3 tangentSpaceNormal = UnpackNormal(tex2D( _NormalMap, i.uv));
    
    float3x3 mtxTangToWorld = {
        i.tangent.x, i.bitangent.x, i.worldNormal.x,
        i.tangent.y, i.bitangent.y, i.worldNormal.y,
        i.tangent.z, i.bitangent.z, i.worldNormal.z,
    };

    float3 N = mul( mtxTangToWorld, tangentSpaceNormal);
    
    float4 specularMap = tex2D(_SpecularMap, i.uv);
    float metallicMap = tex2D(_MetallicMap, i.uv) * 10; 
    fixed4 tex = tex2D(_MainTex, i.uv);
    float metallic = tex * _MetallicIntensity;


    // #ifdef IS_IN_BASEPASS

    //     return float4(metallicMap.xxx,0);
    // #else
    //     return float4(0,0,0,0);
    // #endif


    // diffuse lighting
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); // handles when the light is different types
    // float3 N = normalize(i.worldNormal);
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
        #ifdef IS_IN_BASEPASS
            // float3 reflection = tex2Dlod(_DiffuseIBL, float4(DirToRectilinear(N),0,metallicMap * _MetallicRoughness)); // Texture IBL
            float3 reflectionDir = reflect(-V, i.worldNormal);
            float roughness = 1 - _GlossIntensity;
            roughness *= 1.7 - 0.7 * roughness;
            float4 envSample = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectionDir, _MetallicRoughness * metallicMap * UNITY_SPECCUBE_LOD_STEPS); // Skybox Reflection
		    float3 reflection = DecodeHDR(envSample, unity_SpecCube0_HDR) * _MetallicIntensity;

            reflection = reflection * light * metallicMap ;
            // reflection = lerp(reflection * tex.xyz, 0, metallicMap); 
            
            
            // reflection = 1-metallicMap > 0 ? float3(0,0,0) : reflection;


            // return float4(reflection * _MetallicIntensity, 0);
            // light += reflection * _MetallicIntensity;
            // return float4(reflection * _MetallicIntensity, 0);
        #endif

    #endif

    // specular lighting
    float3 NdotL_Specular = saturate(dot(H, N)); 
    float specularExponent = exp2(_GlossIntensity * 6 + 2 ); // might not be best to do this math in the shader
    float3 specularLight = pow( NdotL_Specular, specularExponent ) * _GlossIntensity * attenuation; // can time with _GlossIntensity for energy conservation aproximation // look into BRDF and inisopotic lense flaire
    specularLight *= light; // using lambert to remove specular in situations when its on the back of the model
    
    #ifdef USE_TOON
        float3 sCol = 1-_UseSpecularLightColor > 0 ? _LightColor0.xyz : (_SpecularColor * _SpecularColor.a);
        specularLight = float4(smoothstep(_GlossAntiAlias ,0.01, specularLight) * sCol * _GlossInensity, 1); // removes gradient
        specularLight = saturate(specularLight);
    #else
        specularLight *= _LightColor0.xyz; // * metallic;
    #endif
    specularLight *= specularMap;

    float fresnel = 1-dot(V, N) * _RimLightIntensity;
    float3 rimLight = float3(fresnel, fresnel, fresnel);

    #ifdef USE_TOON
        rimLight = fresnel * NdotL_Specular;
        rimLight = smoothstep(_RimLightCutOff - _RimLightAntiAlias, _RimLightCutOff + _RimLightAntiAlias, rimLight); // removes gradient 
        rimLight *= 1-_UseSpecularLightColor > 0 ? _LightColor0.xyz : (_ColorRimLight * _ColorRimLight.a);
    #endif
    
    fresnel *= _RimLightToggle;



    #ifdef USE_TOON
        float4 output =  tex * float4(_MainColor, 1) * DiffuseToon;
        output += float4(specularLight, 1) * (_GlossLayer < DiffuseToon);
        output += float4(rimLight, 1);
    #else
        #ifdef IS_IN_BASEPASS
            float4 output = float4(tex.xyz * (light * _MainColor + specularLight + saturate(fresnel) * _ColorRimLight * _ColorRimLight.a) + reflection, 1);
        #else
            float4 output = float4(tex.xyz * (light * _MainColor + specularLight + saturate(fresnel) * _ColorRimLight * _ColorRimLight.a), 1);
        #endif
    #endif
    return output;
}