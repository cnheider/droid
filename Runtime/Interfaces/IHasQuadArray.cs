namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuadArray {
    /// <summary>
    /// </summary>
    UnityEngine.Quaternion[] ObservationArray { get; }

    droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; }
  }
}