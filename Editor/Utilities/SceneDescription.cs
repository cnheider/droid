#if UNITY_EDITOR

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Scene preview.
  ///   https://diegogiacomelli.com.br/unity3d-scenepreview-inspector/
  /// </summary>
  [UnityEditor.CustomEditor(inspectedType : typeof(UnityEditor.SceneAsset))]
  [UnityEditor.CanEditMultipleObjects]
  public class SceneDescription : UnityEditor.Editor {
    /// <summary>
    /// </summary>
    [UnityEngine.RuntimeInitializeOnLoadMethodAttribute]
    public static void CaptureDescription() {
      if (NeodroidSettings.Current.NeodroidGenerateDescriptionsProp) {
        var preview_path =
            GetDescriptionPath(scene_name : UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        #if NEODROID_DEBUG
        UnityEngine.Debug.Log(message : $"Saving scene preview at {preview_path}");
        #endif
        MakeDescription(name : preview_path);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    public static void MakeDescription(string name) {
      var serializer =
          new Newtonsoft.Json.JsonSerializer {NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore};
      var simulation_manager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();

      var path = System.IO.Path.GetDirectoryName(path : name);
      System.IO.Directory.CreateDirectory(path : path);

      using (var sw = new System.IO.StreamWriter(path : name)) {
        using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(textWriter : sw)) {
          serializer.Serialize(jsonWriter : writer, value : simulation_manager.ToString());
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        var scene_names =
            System.Linq.Enumerable.ToArray(source :
                                           System.Linq.Enumerable.OrderBy(source : System.Linq.Enumerable
                                                 .Select(source : this.targets,
                                                         t => ((UnityEditor.SceneAsset)t).name),
                                             n => n));

        var previews_count = scene_names.Length;
        var preview_width = UnityEngine.Screen.width;
        var preview_height =
            (UnityEngine.Screen.height
             - NeodroidEditorConstants._Editor_Margin * 2
             - NeodroidEditorConstants._Preview_Margin * previews_count)
            / previews_count;

        for (var i = 0; i < scene_names.Length; i++) {
          ScenePreview.DrawPreview(index : i,
                                   scene_name : scene_names[i],
                                   width : preview_width,
                                   height : preview_height);
        }
      }

      if (NeodroidSettings.Current.NeodroidGeneratePreviewsProp) {
        var scene_names =
            System.Linq.Enumerable.ToArray(source :
                                           System.Linq.Enumerable.OrderBy(source : System.Linq.Enumerable
                                                 .Select(source : this.targets,
                                                         t => ((UnityEditor.SceneAsset)t).name),
                                             n => n));

        for (var i = 0; i < scene_names.Length; i++) {
          PrintDescription(index : i, scene_name : scene_names[i]);
        }
      }
    }

    static void PrintDescription(int index, string scene_name) {
      var preview_path = GetDescriptionPath(scene_name : scene_name);
      var preview = LoadDescription(file_path : preview_path);

      if (preview != null) {
        UnityEditor.EditorGUILayout.HelpBox(message : preview, type : UnityEditor.MessageType.Info);
      } else {
        UnityEditor.EditorGUILayout.HelpBox(message :
                                            $"There is no image preview for scene {scene_name} at {preview_path}. Please play the scene on editor and image preview will be captured automatically or create the missing path: {NeodroidSettings.Current.NeodroidPreviewsLocationProp}.",
                                            type : UnityEditor.MessageType.Info);
      }
    }

    static string GetDescriptionPath(string scene_name) {
      //return $"{NeodroidEditorInfo.ScenePreviewsLocation}{scene_name}.png";
      return
          $"{UnityEngine.Application.dataPath}/{NeodroidSettings.Current.NeodroidDescriptionLocationProp}{scene_name}.md";
    }

    /// <summary>
    /// </summary>
    /// <param name="file_path"></param>
    /// <returns></returns>
    public static string LoadDescription(string file_path) {
      var description = "The is no description available, press play to generate a description";

      if (System.IO.File.Exists(path : file_path)) {
        using (var sr = new System.IO.StreamReader(path : file_path)) {
          description = sr.ReadToEnd();
        }
      }

      return description;
    }
  }
}
#endif