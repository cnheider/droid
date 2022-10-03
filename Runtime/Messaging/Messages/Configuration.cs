namespace droid.Runtime.Messaging.Messages {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class Configuration : droid.Runtime.Interfaces.IConfigurableConfiguration {
    public Configuration(string configurable_name, float configurable_value, bool sample_random = false) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
      this.SampleRandom = sample_random;
    }

    #region IConfigurableConfiguration Members

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string ConfigurableName { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool SampleRandom { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float ConfigurableValue { get; set; }

    #endregion

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}