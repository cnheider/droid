namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Flare"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Light))]
  public class FlareConfigurable : Configurable {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _color_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space3 {
                                                                        DecimalGranularity = 2,
                                                                        Min =
                                                                            UnityEngine.Vector3.one * 0.7f,
                                                                        Max = UnityEngine.Vector3.one * 1f
                                                                    }
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _int_ind_sha_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space = droid.Runtime.Structs.Space.Space3
                                                                      .TwentyEighty
                                                                  + UnityEngine.Vector3.one * 0.4f
                                                            };

    string _color_b;
    string _color_g;
    string _color_r;
    UnityEngine.Flare _flare;
    string _indirect_multiplier;
    string _intensity;

    UnityEngine.Light _light;
    string _shadow_strength;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._color_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._shadow_strength = this.Identifier + "ShadowStrength";
      this._color_r = this.Identifier + "ColorR";
      this._color_g = this.Identifier + "ColorG";
      this._color_b = this.Identifier + "ColorB";
      this._intensity = this.Identifier + "Intensity";
      this._indirect_multiplier = this.Identifier + "IndirectMultiplier";

      this._light = this.GetComponent<UnityEngine.Light>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._shadow_strength);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_r);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_b);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_g);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._intensity);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._indirect_multiplier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._shadow_strength);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_r);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_g);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_b);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._intensity);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._indirect_multiplier);
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

      if (configuration.ConfigurableName == this._shadow_strength) {
        this._light.shadowStrength = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_r) {
        var c = this._light.color;
        c.r = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._color_g) {
        var c = this._light.color;
        c.g = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._color_b) {
        var c = this._light.color;
        c.b = configuration.ConfigurableValue;
        this._light.color = c;
      } else if (configuration.ConfigurableName == this._intensity) {
        this._light.intensity = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._indirect_multiplier) {
        this._light.bounceIntensity = configuration.ConfigurableValue;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var o = this._int_ind_sha_space.Sample();
      var v = this._color_space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_r,
                         configurable_value : v.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_g,
                         configurable_value : v.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_b,
                         configurable_value : v.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._intensity,
                         configurable_value : o.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._indirect_multiplier,
                         configurable_value : o.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._shadow_strength,
                         configurable_value : o.z)
                   };
    }
  }
}