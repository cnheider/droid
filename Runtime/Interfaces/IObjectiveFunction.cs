namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IObjectiveFunction : IRegisterable {
    /// <summary>
    ///   The range for which the signal might fall
    /// </summary>
    droid.Runtime.Structs.Space.Space1 SignalSpace { get; set; }

    /// <summary>
    ///   Compute signal
    /// </summary>
    /// <returns>floating point signal value</returns>
    float Evaluate();
  }
}