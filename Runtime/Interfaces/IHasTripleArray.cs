namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasTripleArray {
    /// <summary>
    /// </summary>
    UnityEngine.Vector3[] ObservationArray { get; }

    droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; }
  }
}