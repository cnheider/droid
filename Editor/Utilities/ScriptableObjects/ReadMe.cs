namespace droid.Editor.Utilities.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ReadMe : UnityEngine.ScriptableObject {
    /// <summary>
    /// </summary>
    public UnityEngine.Texture2D icon;

    /// <summary>
    /// </summary>
    public bool loadedLayout;

    /// <summary>
    /// </summary>
    public Section[] sections;

    /// <summary>
    /// </summary>
    public string title;

    /// <summary>
    /// </summary>
    [System.SerializableAttribute]
    public class Section {
      /// <summary>
      /// </summary>
      public string heading, text, linkText, url;
    }
  }
}