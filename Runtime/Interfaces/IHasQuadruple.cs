namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasQuadruple {
    /// <summary>
    /// </summary>
    UnityEngine.Quaternion ObservationValue { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space4 QuadSpace { get; }
  }
}