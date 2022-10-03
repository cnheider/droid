#if UNITY_EDITOR

namespace droid.Editor.Windows {
  /// <summary>
  /// </summary>
  public class RenderTextureConfiguratorWindow : UnityEditor.EditorWindow {
    const int _preview_image_size = 100;
    UnityEngine.Texture _icon;

    //float[] _render_texture_height;
    //float[] _render_texture_width;

    System.Collections.Generic.List<UnityEngine.RenderTexture> _render_textures =
        new System.Collections.Generic.List<UnityEngine.RenderTexture>();

    UnityEngine.Vector2 _scroll_position;
    UnityEngine.Vector2 _texture_size;

    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/images.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Tex", image : this._icon, "Window for RenderTexture configuration");
    }

    void OnGUI() {
      this._render_textures.Clear();
      var cameras = FindObjectsOfType<UnityEngine.Camera>();
      foreach (var camera in cameras) {
        if (camera.targetTexture != null) {
          this._render_textures.Add(item : camera.targetTexture);
        }
      }

      this._scroll_position =
          UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      foreach (var render_texture in this._render_textures) {
        UnityEditor.EditorGUILayout.BeginVertical("Box");
        UnityEditor.EditorGUILayout.BeginHorizontal();
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEngine.GUILayout.Label(text : render_texture.name);
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEditor.EditorGUILayout.EndHorizontal();
        UnityEditor.EditorGUILayout.BeginHorizontal();
        UnityEngine.GUILayout.FlexibleSpace();
        var rect = UnityEngine.GUILayoutUtility.GetRect(width : _preview_image_size,
                                                        height : _preview_image_size);
        UnityEditor.EditorGUI.DrawPreviewTexture(position : rect, image : render_texture);
        this._texture_size = new UnityEngine.Vector2(x : render_texture.width, y : render_texture.height);
        UnityEngine.GUILayout.FlexibleSpace();
        UnityEditor.EditorGUILayout.EndHorizontal();
        UnityEditor.EditorGUILayout.EndVertical();
      }

      UnityEditor.EditorGUILayout.EndScrollView();
      this._texture_size =
          UnityEditor.EditorGUILayout.Vector2Field("Set All Render Texture Sizes:",
                                                   value : this._texture_size);
      if (UnityEngine.GUILayout.Button("Apply(Does not work yet)")) {
        // ReSharper disable once UnusedVariable
        foreach (var render_texture in this._render_textures) {
//render_texture.width = (int)_texture_size[0]; //TODO: Read only property to change the asset, it has to be replaced with a new asset
//render_texture.height = (int)_texture_size[1]; // However it is easy to change run time genereted texture by just creating a new texure and replacing the old
        }
      }
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath
                                     + "RenderTextureConfiguratorWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "RenderTextureConfiguratorWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(RenderTextureConfiguratorWindow)); //Show existing window instance. If one doesn't exist, make one.
    }
  }
}
#endif