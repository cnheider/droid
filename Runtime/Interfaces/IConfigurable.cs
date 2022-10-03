namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IConfigurable : IRegisterable {
    /// <summary>
    /// </summary>
    droid.Runtime.Enums.RandomSamplingPhaseEnum RandomSamplingPhaseEnum { get; set; }

    /// <summary>
    /// </summary>
    void UpdateCurrentConfiguration();

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    void ApplyConfiguration(IConfigurableConfiguration configuration);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations();
  }
}