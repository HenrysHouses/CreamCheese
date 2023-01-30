using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ActionGlyphID
{
    public CardActionEnum GlyphType;
    public Texture IconTexture;
    public bool DefaultState = true;
}

[System.Serializable]
class GodGlyphID
{
    public GodActionEnum GlyphType;
    public Texture IconTexture;
    public bool DefaultState = true;
}

public class GlyphController : MonoBehaviour
{
    [SerializeField] ActionGlyphID[] CardGlyphs;
    [SerializeField] GodGlyphID[] GodGlyphs;
    [SerializeField] Renderer meshRenderer;

    public void setGlyph(GodActionEnum type){}
    public void setGlyph(CardActionEnum type)
    {
        for (int i = 0; i < CardGlyphs.Length; i++)
        {
            if(CardGlyphs[i].GlyphType != type)
                continue;
            
            meshRenderer.material.SetTexture("_EmissionTex", CardGlyphs[i].IconTexture);
            meshRenderer.material.SetTexture("_ArtTex", CardGlyphs[i].IconTexture);
            if(CardGlyphs[i].DefaultState)
                meshRenderer.material.SetFloat("_UseEmission", 1);
            else
                meshRenderer.material.SetFloat("_UseEmission", 0);
            return;
        }
    }
}