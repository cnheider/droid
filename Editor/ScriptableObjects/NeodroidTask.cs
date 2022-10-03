#if UNITY_EDITOR
namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// </summary>
  public static class CreateNeodroidTask {
    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName :
                           EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "NeodroidTask")]
    public static void CreateNeodroidTaskAsset() {
      var asset = UnityEngine.ScriptableObject
                             .CreateInstance<droid.Runtime.ScriptableObjects.Deprecated.NeodroidTask>();

      UnityEditor.AssetDatabase.CreateAsset(asset : asset,
                                            path :
                                            $"{droid.Editor.Windows.EditorWindowMenuPath._NewAssetPath}Assets/NewNeodroidTask.asset");
      UnityEditor.AssetDatabase.SaveAssets();

      UnityEditor.EditorUtility.FocusProjectWindow();

      UnityEditor.Selection.activeObject = asset;
    }
  }
}
#endif