using UnityEngine;
using UnityEditor;

public class FontToTexture_Wizard: ScriptableWizard {

    [MenuItem("Tools/Font To Texture")]
    private static void MenuEntryCall() {
        DisplayWizard<FontToTexture_Wizard>("Render Font To Texture");
    }

    public Texture2D FontTextureAsset;
    public string SaveLocation;

    private void OnWizardCreate() {
        SaveTexture2DToPNG(FontTextureAsset, SaveLocation, FontTextureAsset.name + " RenderTexture");
    }

    /// <summary>Saves a Texture2D as a png at file path with name</summary>
    /// <param name="tex">Target texture to save</param>
    /// <param name="filePath">Full system file path</param>
    /// <param name="fileName">Name of the saved file</param>
    public static void SaveTexture2DToPNG(Texture2D tex, string filePath, string fileName)
    {
        Texture2D outTex = new Texture2D(tex.width, tex.height);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                Color pixelColor = tex.GetPixel(i, j);
                Color TargetColor = new Color(pixelColor.a, pixelColor.a, pixelColor.a, pixelColor.a);
                outTex.SetPixel(i, j, TargetColor);
            }
        }

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