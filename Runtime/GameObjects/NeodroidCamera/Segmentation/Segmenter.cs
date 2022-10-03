namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class Segmenter : UnityEngine.MonoBehaviour,
                                    droid.Runtime.Interfaces.IColorProvider {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract System.Collections.Generic.Dictionary<string, UnityEngine.Color> ColorsDict { get; }
  }
}