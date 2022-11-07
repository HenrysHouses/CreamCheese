#ifndef MACROS_INCLUDED
#define MACROS_INCLUDED

#define DECLARE_TOON_VARIABLES \
    float _Detail; \
    float4 _AmbientColor; \
    float _Brightness; \
    float _LightIntensity; \
    float _UseSpecularLightColor; \
    float4 _SpecularColor; \
    float _GlossInensity; \
    float _GlossAntiAlias; \
    float _GlossLayer; \
    float _RimLightCutOff; \
    float _RimLightAntiAlias; 

#define DECLARE_HALFTONE_VARIABLES \
    sampler2D _PatternTex; \
    float4 _PatternTex_ST; \
    float _PatternInvert; \
    float _ScreenResSize; \
    float _ShadeDetail_A; \
    float _ShadeDetail_B; \
    float _PatternOffset; 

#define TAU 6.28318530

#define DECLARE_SCRNPOS_COORDS(idx) float4 scrPos : TEXCOORD##idx;
#define TRANSFER_SCREENPOSITION(o) o.scrPos = ComputeScreenPos(o.pos);

float2 GetScrSpaceUV(float4 scrPos, float4 ScreenParams, float ScreenRez)
{
    float2 _ScreenSpaceUV = scrPos.xy/scrPos.w;
    float x = _ScreenSpaceUV.x * ScreenParams.x/(ScreenRez); // fixes texture stretching on widescreens
    float y = _ScreenSpaceUV.y * ScreenParams.y/(ScreenRez);
    return float2(x,y);
    // return _ScreenSpaceUV;
}

#endif // MACROS_INCLUDED
