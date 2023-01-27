using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [System.Serializable] public struct IconID
    {
        public Texture Texture;
        public CardActionEnum Type;
    }

    public IconID[] IconIDs;
    public Renderer IconMesh;

    public void setIcon(CardActionEnum Type)
    {
        for (int i = 0; i < IconIDs.Length; i++)
        {
            if(Type == IconIDs[i].Type)
            {
                IconMesh.material.SetTexture("_ArtTex", IconIDs[i].Texture);
                IconMesh.material.SetTexture("_EmissionTex", IconIDs[i].Texture);
            }
        }
    }
}
