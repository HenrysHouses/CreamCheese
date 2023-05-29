using UnityEngine;
using UnityEditor;

public class SpriteToTexture_Wizard: ScriptableWizard {

    [MenuItem("Tools/Sprite To Texture")]
    private static void MenuEntryCall() {
        DisplayWizard<SpriteToTexture_Wizard>("Render Sprite To Texture");
    }

    public Sprite SpriteAsset;
    public int size = 250;
    public string SaveLocation;

    private void OnWizardCreate() {
        SaveTexture2DToPNG(SpriteAsset, size, SaveLocation, SpriteAsset.name + " RenderTexture");
    }

    /// <summary>Saves a Texture2D as a png at file path with name</summary>
    /// <param name="tex">Target texture to save</param>
    /// <param name="filePath">Full system file path</param>
    /// <param name="fileName">Name of the saved file</param>
    public static void SaveTexture2DToPNG(Sprite tex, int size, string filePath, string fileName)
    {
        Vector2[] UV = tex.uv;
        Vector2 startUVs = UV[0];        
        Vector2 endUVs = UV[UV.Length-1];        

        Vector2 aspect = Vector2.zero;
        aspect.x = endUVs.x - startUVs.x;
        aspect.y = endUVs.y - startUVs.y;
        if(aspect.x < 0)
            aspect.x *= -1;

        if(aspect.y < 0)
            aspect.y *= -1;

        int totalWidth = (int)(aspect.x/aspect.y);
        Debug.Log(totalWidth);

        Texture2D readTex = tex.texture;
        Texture2D outTex = new Texture2D(totalWidth * size, size);
        for (int i = 0; i < outTex.width; i++)
        {
            for (int j = 0; j < outTex.height; j++)
            {
                int x = (int)(startUVs.x + i * aspect.x);
                int y = (int)(startUVs.y + i * aspect.y);
                Color pixelColor = readTex.GetPixel(x, y);
                // Color TargetColor = new Color(pixelColor.a, pixelColor.a, pixelColor.a, pixelColor.a);
                outTex.SetPixel(i, j, pixelColor);
            }
        }
        // return;

        byte[] bytes = outTex.EncodeToPNG();
        fileName += ".png";
        System.IO.File.WriteAllBytes(filePath + fileName, bytes);
        AssetDatabase.Refresh();
        Debug.Log("Texture Generated to: " + filePath + fileName);

        string[] split = filePath.Split("Assets/");
        EditorUtility.FocusProjectWindow();
        Object CreatedObject = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/"+split[1]+fileName);
        Selection.activeObject = CreatedObject;
    }
}