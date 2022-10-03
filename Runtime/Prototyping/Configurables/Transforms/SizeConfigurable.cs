namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc />
  /// <summary>
  ///   Configurable for scaling
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Size"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class SizeConfigurable : Configurable {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                Space = droid.Runtime.Structs.Space.Space3
                                                                    .ZeroOne
                                                            };

    /// <summary>
    ///   Alpha
    /// </summary>
    string _x;

    /// <summary>
    ///   Blue
    /// </summary>
    string _y;

    /// <summary>
    ///   Green
    /// </summary>
    string _z;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
    }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      //TODO: Denormalize configuration if space is marked as normalised

      #if NEODROID_DEBUG
      if (this.Debugging) {
        droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                         configuration : configuration,
                                                         identifier : this.Identifier);
      }
      #endif
      var local_scale = this.transform.localScale;
      if (configuration.ConfigurableName == this._x) {
        local_scale.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._y) {
        local_scale.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._z) {
        local_scale.z = configuration.ConfigurableValue;
      }

      this.transform.localScale = local_scale;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var v = this._space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._x,
                         configurable_value : v.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._y,
                         configurable_value : v.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._z,
                         configurable_value : v.z)
                   };
    }
  }
}