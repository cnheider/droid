namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasFloatArray {
    /// <summary>
    /// </summary>
    float[] ObservationArray { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; }
  }
}