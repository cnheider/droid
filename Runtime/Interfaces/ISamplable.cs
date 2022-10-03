namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface ISamplable {
    /// <summary>
    /// </summary>
    ISpace Space { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    dynamic Sample();
  }
}