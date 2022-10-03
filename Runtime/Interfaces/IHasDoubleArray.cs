namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasDoubleArray {
    /// <summary>
    /// </summary>
    UnityEngine.Vector2[] ObservationArray { get; }

    droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; }
  }
}