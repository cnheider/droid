namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface ISensor : IRegisterable {
    /// <summary>
    /// </summary>
    System.Collections.Generic.IEnumerable<float> FloatEnumerable { get; }

    /// <summary>
    /// </summary>
    void UpdateObservation();
  }
}