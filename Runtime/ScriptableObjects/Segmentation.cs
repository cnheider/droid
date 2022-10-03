namespace droid.Runtime.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.CreateAssetMenuAttribute(fileName = "Segmentation",
                                        menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath
                                                   + "Segmentation",
                                        order = 1)]
  public class Segmentation : UnityEngine.ScriptableObject {
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.ColorByCategory[] _color_by_categories;
  }
}