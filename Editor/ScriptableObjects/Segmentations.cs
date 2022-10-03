#if UNITY_EDITOR
namespace droid.Editor.ScriptableObjects {
  public static class CreateSegmentations {
    [UnityEditor.MenuItem(itemName : EditorScriptableObjectMenuPath._ScriptableObjectMenuPath
                                     + "Segmentations")]
    public static void CreateSegmentationsAsset() {
      var asset = UnityEngine.ScriptableObject.CreateInstance<droid.Runtime.ScriptableObjects.Segmentation>();

      UnityEditor.AssetDatabase.CreateAsset(asset : asset,
                                            path :
                                            $"{droid.Editor.Windows.EditorWindowMenuPath._NewAssetPath}Assets/NewSegmentations.asset");
      UnityEditor.AssetDatabase.SaveAssets();

      UnityEditor.EditorUtility.FocusProjectWindow();

      UnityEditor.Selection.activeObject = asset;
    }
  }
}
#endif