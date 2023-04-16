#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

[ScriptedImporter(0, "gradtex")]
public class GradientTextureGenerator : ScriptedImporter
{
    public Gradient gradient = new Gradient();
    public int resolution = 16;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Create the texture
        var texture = new Texture2D(resolution, 1);
        texture.wrapMode = TextureWrapMode.Clamp;

        // Fill it
        for (int i = 0; i < resolution; ++i)
        {
            var ratio = i / (float)(resolution - 1);
            texture.SetPixel(i, 0, gradient.Evaluate(ratio));
        }

        // Upload to GPU
        texture.Apply();

        ctx.AddObjectToAsset("texture", texture);
        ctx.SetMainObject(texture);
    }

    [MenuItem("Assets/Create/Gradient Texture", priority = 0)]
    public static void CreateGradientAsset()
    {
        ProjectWindowUtil.CreateAssetWithContent("NewGradient.gradtex", "Gradient dummy file (data is in .meta)");
    }
}
#endif