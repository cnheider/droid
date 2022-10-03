namespace droid.Runtime.Prototyping.Configurables.Selection {
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Difficulty"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : Configurable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "DifficultyConfigurable"; } }

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    public override void UpdateCurrentConfiguration() { }

    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      if (System.Math.Abs(value : configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (System.Math.Abs(value : configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }

    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this.Identifier,
                         configurable_value : this.ConfigurableValueSpace.Sample())
                   };
    }
  }
}