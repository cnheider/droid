#if UNITY_EDITOR
namespace droid.Editor.Utilities.UnityDebug {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class FindMissingScripts : UnityEditor.EditorWindow {
    static int _game_object_count, _components_count, _missing_count;
    [UnityEngine.SerializeField] UnityEngine.Texture2D icon;

    void OnEnable() {
      this.icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/information.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Unity:Debug", image : this.icon, "Window for debugging Unity");
    }

    /// <summary>
    /// </summary>
    public void OnGUI() {
      if (UnityEngine.GUILayout.Button("Find Missing Scripts in selected GameObjects")) {
        FindInSelected();
      }
    }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem("Tools/Debug/FindMissingScripts")]
    [UnityEditor.MenuItem("Window/Debug/FindMissingScripts")]
    public static void ShowWindow() { GetWindow(t : typeof(FindMissingScripts)); }

    static void FindInSelected() {
      var game_objects = UnityEditor.Selection.gameObjects;
      _game_object_count = 0;
      _components_count = 0;
      _missing_count = 0;
      foreach (var g in game_objects) {
        SearchInGameObject(game_object : g);
      }

      UnityEngine.Debug.Log(message :
                            $"Searched {_game_object_count} GameObjects, {_components_count} components, found {_missing_count} missing");
    }

    static void SearchInGameObject(UnityEngine.GameObject game_object) {
      _game_object_count++;
      var components = game_object.GetComponents<UnityEngine.Component>();
      for (var i = 0; i < components.Length; i++) {
        _components_count++;
        if (components[i] == null) {
          _missing_count++;
          var name = game_object.name;
          var parent = game_object.transform;
          while (parent.parent != null) {
            var parent1 = parent.parent;
            name = $"{parent1.name}/{name}";
            parent = parent1;
          }

          UnityEngine.Debug.Log(message : $"{name} has an empty script attached in position: {i}",
                                context : game_object);
        }
      }

      // Now recurse through each child GameObject (if there are any):
      foreach (UnityEngine.Transform child in game_object.transform) {
        SearchInGameObject(game_object : child.gameObject);
      }
    }
  }
}
#endif