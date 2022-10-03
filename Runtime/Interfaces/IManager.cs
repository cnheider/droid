namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IManager : IHasRegister<IEnvironment> {
    /// <summary>
    /// </summary>
    ISimulatorConfiguration SimulatorConfiguration { get; }

    /// <summary>
    /// </summary>
    void Setup();
  }
}