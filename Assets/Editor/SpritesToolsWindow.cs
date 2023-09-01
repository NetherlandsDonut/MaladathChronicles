using UnityEditor;
using UnityEngine;

public class SpritesToolsWindow : EditorWindow
{
    [MenuItem("Window/My Utilities/Sprites Tools")]
    public static void SpriteTools()
    {
        SpritesToolsWindow spriteTools = GetWindow<SpritesToolsWindow>("Sprites Tools", true);
        spriteTools.Show();
        if (Selection.activeObject != null && Selection.activeObject.GetType() == typeof(Texture2D))
            spriteTools.spritesheet = (Texture2D)Selection.activeObject;
    }

    public Texture2D spritesheet;

    private Vector2 m_CommonPivot;
    private string m_Log;

    [System.Obsolete]
    private void OnGUI()
    {

        spritesheet = EditorGUILayout.ObjectField("Spritesheet", spritesheet, typeof(Texture2D), allowSceneObjects: false) as Texture2D;
        m_CommonPivot = EditorGUILayout.Vector2Field("Pivot", m_CommonPivot);
        using (new EditorGUI.DisabledGroupScope(spritesheet == null))
            if (GUILayout.Button("Edit sprites"))
                EditSprites();

        m_Log = EditorGUILayout.TextArea(m_Log);
    }

    [System.Obsolete]
    private void EditSprites()
    {
        if (spritesheet != null)
        {
            string spritesheetPath = AssetDatabase.GetAssetPath(spritesheet);
            if (!string.IsNullOrEmpty(spritesheetPath))
            {
                TextureImporter importer = AssetImporter.GetAtPath(spritesheetPath) as TextureImporter;
                if (importer != null && importer.spritesheet != null && importer.spriteImportMode == SpriteImportMode.Multiple)
                {
                    SpriteMetaData[] spritesMetaData = importer.spritesheet;
                    for (int i = 0; i < spritesMetaData.Length; i++)
                    {
                        SpriteMetaData metaData = spritesMetaData[i];
                        metaData.name = "Font_" + i;
                        metaData.pivot = m_CommonPivot;
                        metaData.alignment = (int)SpriteAlignment.Custom;
                        spritesMetaData[i] = metaData;
                    }
                    importer.spritesheet = spritesMetaData; // seems like this setter internally change stuff (needed)
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                    m_Log += string.Format("Edited {0} sprites in {1}\n", spritesMetaData.Length, spritesheetPath);
                    return;
                }
                m_Log += "Texture is not a spritesheet.\n";
            }
        }
        m_Log += "Could not complete action.\n";
    }
}