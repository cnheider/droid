#if UNITY_EDITOR
namespace droid.Editor.Utilities {
  /// <summary>
  /// </summary>
  public static class MeshSaverEditor {
    const string _menu_path = "CONTEXT" + "/MeshFilter" + "/SaveMesh";

    /// <summary>
    /// </summary>
    /// <param name="menu_command"></param>
    [UnityEditor.MenuItem(itemName : _menu_path)]
    public static void SaveMeshInPlace(UnityEditor.MenuCommand menu_command) {
      var mf = menu_command.context as UnityEngine.MeshFilter;

      if (mf != null) {
        var m = mf.sharedMesh;
        SaveMesh(mesh : m,
                 name : m.name,
                 false,
                 true);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="menu_command"></param>
    [UnityEditor.MenuItem(itemName : _menu_path + "AsANewInstance")]
    public static void SaveMeshNewInstanceItem(UnityEditor.MenuCommand menu_command) {
      var mf = menu_command.context as UnityEngine.MeshFilter;

      if (mf != null) {
        var m = mf.sharedMesh;
        SaveMesh(mesh : m,
                 name : m.name,
                 true,
                 true);
      }
    }

    public static void SaveMesh(UnityEngine.Mesh mesh,
                                string name,
                                bool make_new_instance,
                                bool optimize_mesh) {
      var path = UnityEditor.EditorUtility.SaveFilePanel("Save Separate Mesh Asset",
                                                         "Neodroid/Runtime/Meshes",
                                                         defaultName : name,
                                                         "asset");
      UnityEngine.Debug.Log(message : $"Trying to save mesh to {path}");
      if (string.IsNullOrEmpty(value : path)) {
        return;
      }

      path = UnityEditor.FileUtil.GetProjectRelativePath(path : path);

      var mesh_to_save = make_new_instance ? UnityEngine.Object.Instantiate(original : mesh) : mesh;

      if (optimize_mesh) {
        UnityEditor.MeshUtility.Optimize(mesh : mesh_to_save);
      }

      UnityEditor.AssetDatabase.CreateAsset(asset : mesh_to_save, path : path);
      UnityEditor.AssetDatabase.SaveAssets();
    }
  }
}
#endif