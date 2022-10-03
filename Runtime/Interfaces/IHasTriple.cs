namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasTriple {
    /// <summary>
    /// </summary>
    UnityEngine.Vector3 ObservationValue { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space3 TripleSpace { get; }
  }
}