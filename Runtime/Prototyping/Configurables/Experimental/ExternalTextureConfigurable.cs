namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "ExternalTexture"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class ExternalTextureConfigurable : Configurable {
    [UnityEngine.SerializeField] UnityEngine.Texture _texture = null;

    /// <summary>
    ///   Red
    /// </summary>
    string _texture_str;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return new droid.Runtime.Structs.Space.Sample.SampleSpace1(); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._texture_str = this.Identifier + "Texture"; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._texture_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// n
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(t : this, identifier : this._texture_str);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                         configuration : configuration,
                                                         identifier : this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._texture_str) {
        if (this._texture) {
          this._texture.anisoLevel = (int)configuration.ConfigurableValue;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._texture_str,
                         configurable_value : this.ConfigurableValueSpace.Sample())
                   };
    }
  }
}