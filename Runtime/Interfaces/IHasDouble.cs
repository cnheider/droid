namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasDouble {
    /// <summary>
    /// </summary>
    UnityEngine.Vector2 ObservationValue { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space2 DoubleSpace { get; }
  }
}