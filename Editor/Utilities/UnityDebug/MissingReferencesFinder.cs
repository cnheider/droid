#if UNITY_EDITOR

namespace droid.Editor.Utilities.UnityDebug {
  /// <inheritdoc />
  /// <summary>
  ///   A helper editor script for finding missing references to objects.
  /// </summary>
  public class MissingReferencesFinder : UnityEngine.MonoBehaviour {
    const string _menu_root = "Tools/" + "Missing References/";

    /// <summary>
    ///   Finds all missing references to objects in the currently loaded scene.
    /// </summary>
    [UnityEditor.MenuItem(itemName : _menu_root + "Search in scene", false, 50)]
    public static void FindMissingReferencesInCurrentScene() {
      var scene_objects = GetSceneObjects();
      FindMissingReferences(context : UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                            objects : scene_objects);
    }

    /// <summary>
    ///   Finds all missing references to objects in all enabled scenes in the project.
    ///   This works by loading the scenes one by one and checking for missing object references.
    /// </summary>
    [UnityEditor.MenuItem(itemName : _menu_root + "Search in all scenes", false, 51)]
    public static void MissingSpritesInAllScenes() {
      foreach (var scene in System.Linq.Enumerable.Where(source : UnityEditor.EditorBuildSettings.scenes,
                                                         s => s.enabled)) {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath : scene.path);
        FindMissingReferencesInCurrentScene();
      }
    }

    /// <summary>
    ///   Finds all missing references to objects in assets (objects from the project window).
    /// </summary>
    [UnityEditor.MenuItem(itemName : _menu_root + "Search in assets", false, 52)]
    public static void MissingSpritesInAssets() {
      var all_assets =
          System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Where(source : UnityEditor
                                               .AssetDatabase.GetAllAssetPaths(),
                                           path => path.StartsWith("Assets/")));
      var objs =
          System.Linq.Enumerable.ToArray(source :
                                         System.Linq.Enumerable.Where(source :
                                                                      System.Linq.Enumerable
                                                                          .Select(source : all_assets,
                                                                            a => UnityEditor.AssetDatabase
                                                                                    .LoadAssetAtPath(assetPath
                                                                                      : a,
                                                                                      type : typeof(
                                                                                          UnityEngine.
                                                                                          GameObject)) as
                                                                                UnityEngine.GameObject),
                                                                      a => a != null));

      FindMissingReferences("Project", objects : objs);
    }

    static void FindMissingReferences(string context, UnityEngine.GameObject[] objects) {
      foreach (var go in objects) {
        var components = go.GetComponents<UnityEngine.Component>();

        foreach (var c in components) {
          // Missing components will be null, we can't find their type, etc.
          if (!c) {
            UnityEngine.Debug.LogError(message : $"Missing Component in GO: {GetFullPath(go : go)}",
                                       context : go);
            continue;
          }

          var so = new UnityEditor.SerializedObject(obj : c);
          var sp = so.GetIterator();

          // Iterate over the components' properties.
          while (sp.NextVisible(true)) {
            if (sp.propertyType == UnityEditor.SerializedPropertyType.ObjectReference) {
              if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0) {
                ShowError(context : context,
                          go : go,
                          component_name : c.GetType().Name,
                          property_name : UnityEditor.ObjectNames.NicifyVariableName(name : sp.name));
              }
            }
          }
        }
      }
    }

    static UnityEngine.GameObject[] GetSceneObjects() {
      // Use this method since GameObject.FindObjectsOfType will not return disabled objects.
      return System.Linq.Enumerable.ToArray(source :
                                            System.Linq.Enumerable.Where(source : UnityEngine.Resources
                                                                               .FindObjectsOfTypeAll<
                                                                                   UnityEngine.GameObject>(),
                                                                           go =>
                                                                               string.IsNullOrEmpty(value :
                                                                                 UnityEditor.AssetDatabase
                                                                                     .GetAssetPath(assetObject
                                                                                       : go))
                                                                               && go.hideFlags
                                                                               == UnityEngine.HideFlags
                                                                                   .None));
    }

    static void ShowError(string context,
                          UnityEngine.GameObject go,
                          string component_name,
                          string property_name) {
      const string error_template = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

      UnityEngine.Debug.LogError(message : string.Format(format : error_template,
                                                         GetFullPath(go : go),
                                                         component_name,
                                                         property_name,
                                                         context),
                                 context : go);
    }

    static string GetFullPath(UnityEngine.GameObject go) {
      return go.transform.parent == null
                 ? go.name
                 : $"{GetFullPath(go : go.transform.parent.gameObject)}/{go.name}";
    }
  }
}
#endif