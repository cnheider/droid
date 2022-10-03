#if UNITY_EDITOR
namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// </summary>
  public static class CreateCurriculum {
    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "Curriculum")]
    public static void CreateCurriculumAsset() {
      var asset = UnityEngine.ScriptableObject
                             .CreateInstance<droid.Runtime.ScriptableObjects.Deprecated.Curriculum>();

      UnityEditor.AssetDatabase.CreateAsset(asset : asset,
                                            path :
                                            $"{droid.Editor.Windows.EditorWindowMenuPath._NewAssetPath}Assets/NewCurriculum.asset");
      UnityEditor.AssetDatabase.SaveAssets();

      UnityEditor.EditorUtility.FocusProjectWindow();

      UnityEditor.Selection.activeObject = asset;
    }
  }
}
#endif