using UnityEngine;
using UnityEditor;
using System.IO;

public class BulkShaderAndTextureUpdater : MonoBehaviour
{
    [MenuItem("Tools/Update Shaders and Textures in Folder")]
    public static void UpdateAllAssetsInFolder()
    {
        string folderPath = "Assets/Vox Models/Dynamics"; // Cambia esto a la carpeta que contiene tus .asset
        string shaderName = "Universal Render Pipeline/Unlit";
        string textureProperty = "BaseMap"; // Cambia si tu shader usa otro nombre

        Shader newShader = Shader.Find(shaderName);
        if (newShader == null)
        {
            Debug.LogError("Shader no encontrado: " + shaderName);
            return;
        }

        string[] assetFiles = Directory.GetFiles(folderPath, "*.asset", SearchOption.AllDirectories);
        Debug.Log("#" + assetFiles.Length);
        foreach (string path in assetFiles)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (asset == null)
            {
                Debug.LogWarning("No es un GameObject válido: " + path);
                continue;
            }

            Renderer renderer = asset.GetComponentInChildren<Renderer>();
            if (renderer == null)
            {
                Debug.LogWarning("No se encontró Renderer en: " + path);
                continue;
            }

            Material mat = renderer.sharedMaterial;
            if (mat == null)
            {
                Debug.LogWarning("No se encontró material en: " + path);
                continue;
            }

            mat.shader = newShader;

            // Buscar la textura llamada "Diffuse"
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var obj in subAssets)
            {
                if (obj is Texture2D tex && obj.name == "Diffuse")
                {
                    mat.SetTexture(textureProperty, tex);
                    Debug.Log($"Asignada textura Diffuse al material en: {path}");
                    break;
                }
            }

            EditorUtility.SetDirty(mat);
            AssetDatabase.SaveAssets();
            Debug.Log($"Material actualizado en: {path}");
        }

        Debug.Log("✅ Todos los assets fueron procesados.");
    }
}
