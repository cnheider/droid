namespace droid.Runtime.Structs {
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct ColorByCategory {
    /// <summary>
    /// </summary>
    public string _Category_Name;

    /// <summary>
    /// </summary>
    public UnityEngine.Color _Color;
  }

  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct ColorByInstance {
    /// <summary>
    /// </summary>
    public UnityEngine.GameObject _Game_Object;

    /// <summary>
    /// </summary>
    public UnityEngine.Color _Color;
  }
}